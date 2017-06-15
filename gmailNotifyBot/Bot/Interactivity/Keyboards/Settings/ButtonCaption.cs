using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal static class MainMenuButtonCaption
    {
        public static string Labels => "Labels";

        public static string Permissions => "Permissions";

        public static string Ignore => "Ignore Control";

        public static string About => "About";
    }

    internal static class LabelsMenuButtonCaption
    {
        public static string DisplayLabels => "Display Labels";

        public static string CreateNewLabel => "Create New Label";

        public static string WhiteListDisabled => "Whitelist Labels";

        public static string BlackListDisabled => "Blasklist Labels";

        public static string WhiteListEnabled => $"{Emoji.GRAY_CHECKED_BOX} Whitelist Labels";

        public static string BlackListEnabled => $"{Emoji.GRAY_CHECKED_BOX} Blasklist Labels";

    }

    internal static class IgnoreMenuButtonCaption
    {
        public static string Show => "Show Ignore List";

        public static string Add => "Add to Ignore";

        public static string Remove => "Remove from Ignore";
    }

    internal static class PermissionsMenuButtonCaption
    {
        public static string ChangePermissions => "Change Permissions to";

        public static string RevokePermissions => "Revoke Permissions";

        public static string RevokeViaWeb => "Revoke Permissions via web";
    }

    internal static class GeneralButtonCaption
    {
        public static string Back => $"{Emoji.LEFT_ARROW} Back";
    }

    internal static class LabelActionsButtonCaption
    {
        public static string RemoveLabel => "Remove Label";

        public static string EditLabel => "Edit Label";
    }

    internal static class LabelListButtonCaption
    {
        public static string UseBlacklist => "Use Blacklist Mode";

        public static string UseWhitelist => "Use Whitelist Mode";
    }
}