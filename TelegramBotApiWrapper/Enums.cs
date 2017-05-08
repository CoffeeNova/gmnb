using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{

    public enum UpdateType
    {
        [Update("")]
        AllUpdates,
        [Update("message")]
        Message,
        [Update("edited_message")]
        EditedMessage,
        [Update("channel_post")]
        ChannelPost,
        [Update("edited_channel_post")]
        EditedChannelPost,
        [Update("inline_query")]
        InlineQuery,
        [Update("chosen_inline_result")]
        ChosenInlineResult,
        [Update("callback_query")]
        CallbackQuery
    }

    public enum Action
    {
        [Action("typing")]
        Typing,
        [Action("upload_photo ")]
        UploadPhoto,
        [Action("record_video")]
        RecordVideo,
        [Action("upload_video")]
        UploadVideo,
        [Action("record_audio")]
        RecordAudio,
        [Action("upload_audio")]
        UploadAudio,
        [Action("upload_document")]
        UploadDocument,
        [Action("find_location")]
        FindLocation
    }
}