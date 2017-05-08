using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;

namespace CoffeeJelly.TelegramBotApiWrapper.Types
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatType
    {
        [EnumMember(Value = "private")]
        Private,
        [EnumMember(Value = "group")]
        Group,
        [EnumMember(Value = "supergroup")]
        Supergroup,
        [EnumMember(Value = "channel")]
        Channel
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageEntityType
    {
        [EnumMember(Value = "mention")]
        Mention,
        [EnumMember(Value = "hashtag")]
        Hashtag,
        [EnumMember(Value = "bot_command")]
        BotCommand,
        [EnumMember(Value = "url")]
        Url,
        [EnumMember(Value = "email")]
        Email,
        [EnumMember(Value = "bold")]
        Bold,
        [EnumMember(Value = "italic")]
        Italic,
        [EnumMember(Value = "code")]
        Code,
        [EnumMember(Value = "pre")]
        Pre,
        [EnumMember(Value = "text_link")]
        TextLink,
        [EnumMember(Value = "text_mention")]
        TextMention
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatMemberStatus
    {
        [EnumMember(Value = "creator")]
        Creator,
        [EnumMember(Value = "administrator")]
        Administrator,
        [EnumMember(Value = "member")]
        Member,
        [EnumMember(Value = "left")]
        Left,
        [EnumMember(Value = "kicked")]
        Kicked
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ParseMode
    {
        [EnumMember(Value = "Markdown")]
        Markdown,
        [EnumMember(Value = "HTML")]
        Html
    }

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

    public enum ChatAction
    {
        [ChatAction("typing")]
        Typing,
        [ChatAction("upload_photo ")]
        UploadPhoto,
        [ChatAction("record_video")]
        RecordVideo,
        [ChatAction("upload_video")]
        UploadVideo,
        [ChatAction("record_audio")]
        RecordAudio,
        [ChatAction("upload_audio")]
        UploadAudio,
        [ChatAction("upload_document")]
        UploadDocument,
        [ChatAction("find_location")]
        FindLocation
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MimeType
    {
        [EnumMember(Value = "text/html")]
        TextHtml,
        [EnumMember(Value = "video/mp4")]
        VideoMp4,
        [EnumMember(Value = "application/pdf")]
        ApplicationPdf,
        [EnumMember(Value = "application/zip")]
        ApplicationZip
    }
}