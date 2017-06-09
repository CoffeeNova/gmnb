﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Models
{
    public class UserDbInitializer : CreateDatabaseIfNotExists<GmailBotDbContext>
    {
        protected override void Seed(GmailBotDbContext db)
        {
            db.Users.Add(new UserModel
            {
                UserId = 1,
                FirstName = "testFirst",
                LastName = "testLast",
                Username = "testUsername",
                IssuedTimeUtc = DateTime.UtcNow,
                Email = "test@gmail.com",
                AccessToken = "testaccess",
                EmailVerified = true,
                ExpiresIn = 100,
                GoogleUserId = "1",
                IdToken = "1",
                RefreshToken = "testrefresh",
                TokenType = "test"
            });
            db.PendingUser.Add(new PendingUserModel
            {
                UserId = 1,
                JoinTimeUtc = DateTime.UtcNow
            });
            db.UserSettings.Add(new UserSettingsModel
            {
                MailNotification = false,
                UserId = 1,
                Access = UserAccess.Full,
                IgnoreList = new List<IgnoreModel>
                {
                 new IgnoreModel { Address = "testadr1@gmail.com"},
                 new IgnoreModel {Address = "testadr2@gmail.com" }
                },
                Expiration = 100,
                HistoryId = 1
            });
            db.NmStore.Add(new NmStoreModel
            {
                UserId = 0,
                MessageId = 0,
                To = new List<ToModel>
                {
                    new ToModel {Address = "to1@gmail.com" },
                    new ToModel {Address = "to2@gmail.com" }
                },
                Cc = new List<CcModel>
                {
                    new CcModel {Address = "cc@gmail.com" }
                },
                Bcc = new List<BccModel>
                {
                    new BccModel {Address = "bcc@gmail.com" }
                },
                Message = "testMessage",
                Subject = "testSubj",
                File = new List<FileModel>
                {
                    new FileModel { FileId = "testFileId1", AttachId = "testAttach1", OriginalName = "testFileName1"},
                    new FileModel { FileId = "testFileId2", AttachId = "testAttach2", OriginalName = "testFileName2"}
                }
            });
            base.Seed(db);
        }
    }
}