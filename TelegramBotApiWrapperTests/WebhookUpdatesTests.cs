using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.TelegramBotApiWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;

namespace CoffeeJelly.TelegramBotApiWrapper.Tests
{
    [TestClass()]
    public class WebhookUpdatesTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _config = new ComparisonConfig
            {
                CompareChildren = true,
                CompareFields = false,
                CompareReadOnly = true,
                ComparePrivateFields = false,
                ComparePrivateProperties = false,
                CompareProperties = true,
                MaxDifferences = 50,
                MembersToIgnore = new List<string> { "MessageId", "Date", "ForwardDate", "UpdateId" }
            };
            var rm = new ResourceManager("CoffeeJelly.TelegramBotApiWrapperTests.Token", Assembly.GetExecutingAssembly());
            _token = rm.GetString("testToken");
            _url = rm.GetString("webhookUrl");
        }

        [TestMethod()]
        public void HandleTelegramRequestTest()
        {
            var update =
                "{\"update_id\":982338622,\"message\":{\"message_id\":4574,\"from\":{\"id\":170181775,\"first_name\":\"Coffee\",\"last_name\":\"Jelly\"," +
                "\"username\":\"CoffeeJelly\",\"language_code\":\"en\"},\"chat\":{\"id\":170181775,\"first_name\":\"Coffee\",\"last_name\":\"Jelly\",\"username\":" +
                "\"CoffeeJelly\",\"type\":\"private\"},\"date\":1498132539,\"text\":\"/settings\",\"entities\":[{\"type\":\"bot_command\",\"offset\":0,\"length\":9}]}}";
            var webhook = new WebhookUpdates(_token);
            webhook.UpdatesArrivedEvent += Webhook_UpdatesArrivedEvent;
            webhook.HandleTelegramRequest(update);
        }

        private void Webhook_UpdatesArrivedEvent(IUpdate updates)
        {
            Assert.IsTrue(true);
        }

        private static ComparisonConfig _config;
        private static string _token;
        private static string _url;
    }
}