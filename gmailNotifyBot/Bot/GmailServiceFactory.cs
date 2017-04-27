using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Attributes;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Microsoft.Owin.Security.Twitter.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class GmailServiceFactory
    {
        public static GmailServiceFactory GetInstanse(Secrets secrets)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new GmailServiceFactory(secrets);
                }
            }
            return Instance;
        }

        public async Task RestoreServicesFromStore()
        {
            var gmailDbContextWorker = new GmailDbContextWorker();
            var users = await gmailDbContextWorker.GetAllUsersAsync();

            await Task.Run(() =>
            {
                users.ForEach(userModel =>
                {
                    var userSettingsModel = gmailDbContextWorker.FindUserSettings(userModel.UserId);
                    Instance_AuthorizationRegistredEvent(userModel, userSettingsModel);

                });
            });

        }

        private GmailServiceFactory(Secrets secrets)
        {
            _clientSecrets = new ClientSecrets { ClientId = secrets.ClientId, ClientSecret = secrets.Secret };
            Authorizer.Instance.AuthorizationRegistredEvent += Instance_AuthorizationRegistredEvent;
        }

        private  void Instance_AuthorizationRegistredEvent(UserModel userModel, UserSettingsModel userSettingsModel)
        {
            if (ServiceExists(userModel.UserId.ToString()))
                return;

            var token = new TokenResponse
            {
                RefreshToken = userModel.RefreshToken,
                AccessToken = userModel.AccessToken,
                ExpiresInSeconds = userModel.ExpiresIn,
                TokenType = userModel.TokenType,
                IssuedUtc = userModel.IssuedTimeUtc
            };
            var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = _clientSecrets,
                        Scopes = UserAccessAttribute.GetScopesValue(userSettingsModel.Access),
                        DataStore = new DbDataStore()
                    }),
                userModel.UserId.ToString(),
                token);
            Services.Add(new GmailService(new Google.Apis.Services.BaseClientService.Initializer
            {
                ApiKey = "AIzaSyCrVK6UQ4h45WH1DQX6BXNMEIikoT_HEwI",
                ApplicationName = "gmnb", //this.GetType().ToString()
                HttpClientInitializer = credentials
            }));
        }

        private bool ServiceExists(string userId)
        {
            return Services.Any(s => (s.HttpClientInitializer as UserCredential).UserId == userId);
        }

        private ClientSecrets _clientSecrets;

        private static readonly object _locker = new object();

        public static GmailServiceFactory Instance { get; private set; }

        public List<GmailService> Services { get; } = new List<GmailService>();
    }
}