﻿// ReSharper disable All

namespace MyTelegram.Handlers.Stories;

///<summary>
/// See <a href="https://corefork.telegram.org/method/stories.getPinnedStories" />
///</summary>
internal sealed class GetPinnedStoriesHandler : RpcResultObjectHandler<MyTelegram.Schema.Stories.RequestGetPinnedStories, MyTelegram.Schema.Stories.IStories>,
    Stories.IGetPinnedStoriesHandler
{
    protected override Task<MyTelegram.Schema.Stories.IStories> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Stories.RequestGetPinnedStories obj)
    {
        throw new NotImplementedException();
    }
}
