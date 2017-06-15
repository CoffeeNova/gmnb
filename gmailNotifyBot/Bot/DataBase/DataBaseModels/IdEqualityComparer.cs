using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class IdEqualityComparer<T> : IEqualityComparer<T> where T: ICommonModelKey
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}