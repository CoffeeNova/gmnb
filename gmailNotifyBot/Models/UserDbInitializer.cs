using System;
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
                Access = UserAccess.FULL,
                UseWhitelist = false,
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
                    new ToModel {Email = "to1@gmail.com", Name="testToName1" },
                    new ToModel {Email = "to2@gmail.com", Name="testToName2"  }
                },
                Cc = new List<CcModel>
                {
                    new CcModel {Email = "cc@gmail.com", Name="testCcName1" }
                },
                Bcc = new List<BccModel>
                {
                    new BccModel {Email = "bcc@gmail.com", Name="testBccName1"  }
                },
                Message = "testMessage",
                Subject = "testSubj",
                File = new List<FileModel>
                {
                    new FileModel { FileId = "testFileId1", AttachId = "testAttach1", OriginalName = "testFileName1"},
                    new FileModel { FileId = "testFileId2", AttachId = "testAttach2", OriginalName = "testFileName2"}
                }
            });
            db.LogEntry.Add(new LogEntryModel
            {
                CallSite="testCs",
                Date="testDate",
                Exception ="testEx",
                Level="testLvl",
                Logger="testLogger",
                MachineName="testMn",
                Message="testMessage",
                StackTrace="testSt",
                Thread="testThread",
                Username="testUsername"
            });
            base.Seed(db);
        }
    }
}