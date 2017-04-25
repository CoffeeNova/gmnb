using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Microsoft.Owin.Security.Twitter.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class TestClass
    {
        public TestClass(Secrets secrets, List<string> scopes)
        {
            _clientSecrets = new ClientSecrets {ClientId = secrets.ClientId, ClientSecret = secrets.Secret};
            Scopes = scopes;
            Authorizer.Instance.AuthorizationRegistredEvent += Instance_AuthorizationRegistredEvent;
        }

        private void Instance_AuthorizationRegistredEvent(DataBase.DataBaseModels.UserModel userModel)
        {
            var token = new TokenResponse
            {
                RefreshToken = userModel.RefreshToken,
                AccessToken = userModel.AccessToken,
                ExpiresInSeconds= userModel.ExpiresIn,
                TokenType = userModel.TokenType,
                IssuedUtc = userModel.IssuedTimeUtc
            };
            var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = _clientSecrets,
                        Scopes = Scopes,
                        DataStore 
                    }),
                "user",
                token);

            var service = new GmailService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = "",
                ApplicationName = "", //this.GetType().ToString()
                HttpClientInitializer = _credential
            });

           // var emailListRequest = service.Users.Messages.Get
        }

        private UserCredential _credential;
        private ClientSecrets _clientSecrets;

        public Secrets Secrets { get; set; }
        public List<string> Scopes { get; set; }
    }
}