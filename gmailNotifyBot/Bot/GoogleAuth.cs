using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class GoogleAuth
    {

        public GoogleAuth()
        {
            //_refreshAccessTokenTimer = new Timer(
            //    state => RefreshAccessToken(),
            //    null,
            //    0,
            //    (_accessTokenExpiresTime - TimeSpan.FromMinutes(1)).Milliseconds
            //    );
        }
        public static GoogleAuth Get(string response)
        {
            var result = JsonConvert.DeserializeObject(response) as GoogleAuth;
            if (result == null) return null;

            result.Created = DateTime.Now;   // DateTime.Now.Add(new TimeSpan(-2, 0, 0)); //For testing force refresh.
            return result;
        }

        public void RefreshAccessToken()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            string postData =
                $"client_id={this.ClientId}&client_secret={this.Secret}&refresh_token={this.RefreshToken}&grant_type=refresh_token";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var refreshResponse = GoogleAuth.Get(responseString);
            this._accessToken = refreshResponse.AccessToken;
            this.Created = DateTime.Now;
        }

        public static GoogleAuth Exchange(string authCode, string clientid, string secret, string redirectURI)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");

            string postData =
                $"code={authCode}&client_id={clientid}&client_secret={secret}&redirect_uri={redirectURI}&grant_type=authorization_code";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var x = GoogleAuth.Get(responseString);

            x.ClientId = clientid;
            x.Secret = secret;

            return x;

        }


        private string _accessToken;
        public string AccessToken
        {
            get
            {
                // Access token lasts an hour if its expired we get a new one.
                if (DateTime.Now.Subtract(Created).Hours > 1)
                {
                    RefreshAccessToken();
                }
                return _accessToken;
            }
            set { _accessToken = value; }
        }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string ExpiresIn { get; set; }
        public DateTime Created { get; set; }

        //private Timer _refreshAccessTokenTimer;
        //private static readonly TimeSpan _accessTokenExpiresTime = TimeSpan.FromHours(1);

    }
}
