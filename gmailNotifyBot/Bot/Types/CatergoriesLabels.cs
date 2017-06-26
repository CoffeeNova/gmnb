using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Types
{
    public struct CatergoriesLabels
    {
        public static List<LabelInfo> List = new List<LabelInfo>
        {
            new LabelInfo {LabelId = "CATEGORY_PERSONAL", Name = "Categories" },
            new LabelInfo {LabelId = "CATEGORY_SOCIAL", Name = "Social" },
            new LabelInfo {LabelId = "CATEGORY_UPDATES", Name = "Updates" },
            new LabelInfo {LabelId = "CATEGORY_FORUMS", Name = "Forums" },
            new LabelInfo {LabelId = "CATEGORY_PROMOTIONS", Name = "Promotions" }
        };

        public static string ReturnLabelName(string labelId)
        {
            return List.FirstOrDefault(l => l.LabelId == labelId)?.Name;
        }
    }
}