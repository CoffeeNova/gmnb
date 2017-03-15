using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using CoffeeJelly.gmnb.Bot;

namespace CoffeeJelly.gmnb
{
    class Program
    {
        static void Main(string[] args)
        {
            //FOR TEST ONLY! NOT SECURE!
            string token = "252886092:AAHxtq8ZINX6WJXcT-MuQFoarH9-8Ppntl8";

            GmnbRequests.RequestsArrivedEvent += GmnbRequests_RequestsArrivedEvent;
            GmnbRequests testreq = new GmnbRequests(token);
            while (true)
            {
                Thread.Sleep(100);
            }
        }

        private static void GmnbRequests_RequestsArrivedEvent(IGmnbRequests requests)
        {
            //Console.WriteLine(requests.LastUpdateId);
            foreach (var r in requests.Requests)
            {
                Console.WriteLine(r);
            }
            DateTime dr = new DateTime();
        }

    }
}
