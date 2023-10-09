﻿// ReSharper disable All

namespace MyTelegram.Handlers.Messages;

///<summary>
/// Answer an inline query, for bots only
/// <para>Possible errors</para>
/// Code Type Description
/// 400 ARTICLE_TITLE_EMPTY The title of the article is empty.
/// 400 AUDIO_CONTENT_URL_EMPTY The remote URL specified in the content field is empty.
/// 400 AUDIO_TITLE_EMPTY An empty audio title was provided.
/// 400 BUTTON_DATA_INVALID The data of one or more of the buttons you provided is invalid.
/// 400 BUTTON_TYPE_INVALID The type of one or more of the buttons you provided is invalid.
/// 400 BUTTON_URL_INVALID Button URL invalid.
/// 400 DOCUMENT_INVALID The specified document is invalid.
/// 400 FILE_CONTENT_TYPE_INVALID File content-type is invalid.
/// 400 FILE_TITLE_EMPTY An empty file title was specified.
/// 400 GIF_CONTENT_TYPE_INVALID GIF content-type invalid.
/// 400 MESSAGE_EMPTY The provided message is empty.
/// 400 MESSAGE_TOO_LONG The provided message is too long.
/// 400 NEXT_OFFSET_INVALID The specified offset is longer than 64 bytes.
/// 400 PHOTO_CONTENT_TYPE_INVALID Photo mime-type invalid.
/// 400 PHOTO_CONTENT_URL_EMPTY Photo URL invalid.
/// 400 PHOTO_INVALID Photo invalid.
/// 400 PHOTO_THUMB_URL_EMPTY Photo thumbnail URL is empty.
/// 400 QUERY_ID_INVALID The query ID is invalid.
/// 400 REPLY_MARKUP_INVALID The provided reply markup is invalid.
/// 400 RESULTS_TOO_MUCH Too many results were provided.
/// 400 RESULT_ID_DUPLICATE You provided a duplicate result ID.
/// 400 RESULT_ID_INVALID One of the specified result IDs is invalid.
/// 400 RESULT_TYPE_INVALID Result type invalid.
/// 400 SEND_MESSAGE_MEDIA_INVALID Invalid media provided.
/// 400 SEND_MESSAGE_TYPE_INVALID The message type is invalid.
/// 400 START_PARAM_EMPTY The start parameter is empty.
/// 400 START_PARAM_INVALID Start parameter invalid.
/// 400 STICKER_DOCUMENT_INVALID The specified sticker document is invalid.
/// 400 SWITCH_PM_TEXT_EMPTY The switch_pm.text field was empty.
/// 400 URL_INVALID Invalid URL provided.
/// 403 USER_BOT_INVALID User accounts must provide the <code>bot</code> method parameter when calling this method. If there is no such method parameter, this method can only be invoked by bot accounts.
/// 400 VIDEO_TITLE_EMPTY The specified video title is empty.
/// 400 WEBDOCUMENT_INVALID Invalid webdocument URL provided.
/// 400 WEBDOCUMENT_MIME_INVALID Invalid webdocument mime type provided.
/// 400 WEBDOCUMENT_SIZE_TOO_BIG Webdocument is too big!
/// 400 WEBDOCUMENT_URL_INVALID The specified webdocument URL is invalid.
/// See <a href="https://corefork.telegram.org/method/messages.setInlineBotResults" />
///</summary>
internal sealed class SetInlineBotResultsHandler : RpcResultObjectHandler<MyTelegram.Schema.Messages.RequestSetInlineBotResults, IBool>,
    Messages.ISetInlineBotResultsHandler
{
    protected override Task<IBool> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Messages.RequestSetInlineBotResults obj)
    {
        throw new NotImplementedException();
    }
}
