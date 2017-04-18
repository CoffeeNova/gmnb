﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public static class UserContextWorker
    {
        public static UserModel FindUser(Chat user)
        {
            user.NullInspect(nameof(user));

            using (var db = new UserContext())
            {
                return db.Users.FirstOrDefault(u => u.UserId == user.Id);
            }
        }

        public static Task<UserModel> FindUserAsync(Chat user)
        {
            user.NullInspect(nameof(user));

            return Task.Run(() => FindUser(user));
        }

        public static UserModel AddNewUser(Chat user)
        {
            user.NullInspect(nameof(user));

            using (var db = new UserContext())
            {
                var newModel = db.Users.Add(new UserModel(user));
                db.SaveChanges();
                return newModel;
            }
        }

        public static Task<UserModel> AddNewUserAsync(Chat user)
        {
            user.NullInspect(nameof(user));

            return Task.Run(() => AddNewUser(user));
        }

        public static PendingUserModel Queue(long userId, string state)
        {
            state.NullInspect(nameof(state));

            using (var db = new UserContext())
            {
                var newModel = db.PendingUser.Add(new PendingUserModel
                {
                    UserId = userId,
                    State = state,
                    JoinTime = DateTime.Now
                });
                db.SaveChanges();
                return newModel;
            }
        }

        public static Task<PendingUserModel> QueueAsync(long userId, string state)
        {
            state.NullInspect(nameof(state));

            return Task.Run(() => Queue(userId, state));
        }

        public static void RemoveFromQueue(PendingUserModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new UserContext())
            {
                db.PendingUser.Remove(model);
                db.SaveChanges();
            }
        }

        public static Task RemoveFromQueueAsync(PendingUserModel model)
        {
            model.NullInspect(nameof(model));

            return Task.Run(() => RemoveFromQueue(model));
        }

        public static PendingUserModel UpdateRecordJoinTime(long id, DateTime time)
        {
            using (var db = new UserContext())
            {
                var query = db.PendingUser.Find(id);
                if (query != null)
                {
                    query.JoinTime = time;
                    db.SaveChanges();
                }
                return query;
            }
        }

        public static Task<PendingUserModel> UpdateRecordJoinTimeAsync(long id, DateTime time)
        {
            return Task.Run(() => UpdateRecordJoinTime(id, time));
        }

        public static PendingUserModel FindPendingUser(long userId)
        {
            using (var db = new UserContext())
            {
                return db.PendingUser.FirstOrDefault(p => p.UserId == userId);
            }
        }

        public static Task<PendingUserModel> FindPendingUserAsync(long userId)
        {
            return Task.Run(() => FindPendingUser(userId));
        }
    }
}