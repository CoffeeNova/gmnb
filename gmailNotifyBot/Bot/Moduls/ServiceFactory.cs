﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Attributes;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public sealed class ServiceFactory
    {
        public static ServiceFactory GetInstanse(Secrets secrets)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new ServiceFactory(secrets);
                }
            }
            return Instance;
        }

        public async Task RestoreServicesFromRepositoryAsync()
        {
            var gmailDbContextWorker = new GmailDbContextWorker();
            var users = await gmailDbContextWorker.GetAllUsersAsync();

            if (users == null) return;
            await Task.Run(() =>
            {
                users.ForEach(userModel =>
                {
                    var userSettingsModel = gmailDbContextWorker.FindUserSettings(userModel.UserId);
                    try
                    {
                        Instance_AuthorizationRegistredEvent(userModel, userSettingsModel);
                    }
                    catch (Exception ex)
                    {
                        gmailDbContextWorker.RemoveUserRecord(userModel);
                        LogMaker.Log(Logger, ex);
                    }

                });
            });
        }


        public void RestoreServicesFromRepository()
        {
            var gmailDbContextWorker = new GmailDbContextWorker();
            var users = gmailDbContextWorker.GetAllUsers();

            users?.ForEach(userModel =>
            {
                var userSettingsModel = gmailDbContextWorker.FindUserSettings(userModel.UserId);
                try
                {
                    Instance_AuthorizationRegistredEvent(userModel, userSettingsModel);
                }
                catch (Exception ex)
                {
                    gmailDbContextWorker.RemoveUserRecord(userModel);
                    LogMaker.Log(Logger, ex);
                }

            });
        }

        private ServiceFactory(Secrets secrets)
        {
            _clientSecrets = new ClientSecrets { ClientId = secrets.ClientId, ClientSecret = secrets.Secret };
            Authorizer.Instance.AuthorizationRegistredEvent += Instance_AuthorizationRegistredEvent;
        }

        private void Instance_AuthorizationRegistredEvent(UserModel userModel, UserSettingsModel userSettingsModel)
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
            var credentials = new BotUserCredential(new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = _clientSecrets,
                        Scopes = UserAccessAttribute.GetScopesValue(userSettingsModel.Access),
                        DataStore = new DbDataStore()
                    }),
                userModel,
                token);
            var serviceInitializer = new Google.Apis.Services.BaseClientService.Initializer
            {
                ApiKey = "AIzaSyCrVK6UQ4h45WH1DQX6BXNMEIikoT_HEwI",
                ApplicationName = "gmnb", //this.GetType().ToString()
                HttpClientInitializer = credentials
            };
            ServiceCollection.Add(new Service(credentials, serviceInitializer));
        }

        private bool ServiceExists(string userId)
        {
            return ServiceCollection.Any(s => s.UserCredential.UserId == userId);
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ClientSecrets _clientSecrets;

        private static readonly object _locker = new object();

        public static ServiceFactory Instance { get; private set; }

        public List<Service> ServiceCollection { get; } = new List<Service>();
    }

    public class Service : ISender
    {
        public Service(BotUserCredential userCredential, BaseClientService.Initializer initializer)
        {
            UserCredential = userCredential;
            GmailService = new GmailService(initializer);
            Oauth2Service = new Oauth2Service(initializer);
        }
        public GmailService GmailService { get; set; }
        public Oauth2Service Oauth2Service { get; set; }
        public BotUserCredential UserCredential { get; set; }


        public User From
        {
            get
            {
                return UserCredential.From;
            }
            set
            {
            }
        }
    }

    public class BotUserCredential : UserCredential
    {
        public BotUserCredential(IAuthorizationCodeFlow flow, User user, TokenResponse token) : base(flow, user.Id.ToString(), token)
        {
            From = user;
        }

        public User From { get; }

    }
}