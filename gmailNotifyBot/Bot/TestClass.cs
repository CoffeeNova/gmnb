using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
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

        private GmailServiceFactory(Secrets secrets)
        {
            _clientSecrets = new ClientSecrets { ClientId = secrets.ClientId, ClientSecret = secrets.Secret };
            Authorizer.Instance.AuthorizationRegistredEvent += Instance_AuthorizationRegistredEvent;
        }

        private void Instance_AuthorizationRegistredEvent(UserModel userModel, UserSettingsModel userSettingsModel)
        {
            var token = new TokenResponse
            {
                RefreshToken = userModel.RefreshToken,
                AccessToken = userModel.AccessToken,
                ExpiresInSeconds = userModel.ExpiresIn,
                TokenType = userModel.TokenType,
                IssuedUtc = userModel.IssuedTimeUtc
            };
            Credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = _clientSecrets,
                        Scopes = Scopes,
                        DataStore = new DbDataStore()
                    }),
                "user",
                token);
            Service = new GmailService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCrVK6UQ4h45WH1DQX6BXNMEIikoT_HEwI",
                ApplicationName = "gmnb", //this.GetType().ToString()
                HttpClientInitializer = Credentials
            });
            var userId = Service.Name;
            // var emailListRequest = Service.Users.Messages.List()
        }

        private ClientSecrets _clientSecrets;

        private static readonly object _locker = new object();

        public static GmailServiceFactory Instance { get; private set; }

        public List<string> Scopes { get; set; }

        public UserCredential Credentials { get; private set; }

        public GmailService Service { get; private set; }
    }
}