﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegram.Domain.Sagas;

[JsonConverter(typeof(SystemTextJsonSingleValueObjectConverter<SendMessageSagaId>))]
public class SendMessageSagaId : SingleValueObject<string>, ISagaId
{
    public SendMessageSagaId(string value) : base(value)
    {
    }
}

public class SendMessageSagaLocator : DefaultSagaLocator<SendMessageSaga, SendMessageSagaId>
{
    protected override SendMessageSagaId CreateSagaId(string requestId)
    {
        return new SendMessageSagaId(requestId);
    }
}

public class SendMessageSagaState : AggregateState<SendMessageSaga, SendMessageSagaId, SendMessageSagaState>,
    IApply<SendMessageSagaStartedEvent>,
    IApply<SendOutboxMessageCompletedEvent2>,
    IApply<ReceiveInboxMessageCompletedEvent2>
{
    public RequestInfo RequestInfo { get; set; } = default!;
    public MessageItem MessageItem { get; set; } = default!;
    public List<long>? MentionedUserIds { get; private set; }
    public int GroupItemCount { get; set; }
    public long? LinkedChannelId { get; set; }
    //public List<long>? BotUserIds { get; set; }
    public long ReplyToMessageSavedFromPeerId { get; private set; }
    //public int ReplyToMsgId { get; set; }
    //public bool ForwardFromLinkedChannel { get; set; }
    //public int Pts { get; private set; }

    //public List<ReplyToMsgItem>? ReplyToMsgItems { get; private set; }
    public List<long>? ChatMembers { get; private set; } = new();
    public List<InboxItem> InboxItems { get; private set; } = new();

    public Dictionary<long, int> ReplyToMsgItems { get; private set; } = new();

    public void Apply(SendMessageSagaStartedEvent aggregateEvent)
    {
        RequestInfo = aggregateEvent.RequestInfo;
        MessageItem = aggregateEvent.MessageItem;
        MentionedUserIds = aggregateEvent.MentionedUserIds;
        GroupItemCount = aggregateEvent.GroupItemCount;
        LinkedChannelId = aggregateEvent.LinkedChannelId;
        ChatMembers = aggregateEvent.ChatMembers;
        //ReplyToMsgItems=aggregateEvent.ReplyToMsgItems;

        if (aggregateEvent.ReplyToMsgItems?.Count > 0)
        {
            ReplyToMsgItems = aggregateEvent.ReplyToMsgItems.ToDictionary(k => k.UserId, v => v.MessageId);
        }
    }

    public void Apply(SendOutboxMessageCompletedEvent2 aggregateEvent)
    {
        //throw new NotImplementedException();
    }

    public void Apply(ReceiveInboxMessageCompletedEvent2 aggregateEvent)
    {
        //throw new NotImplementedException();
        InboxItems.Add(new(aggregateEvent.MessageItem.OwnerPeer.PeerId, aggregateEvent.MessageItem.MessageId));
    }

    public bool IsCreateInboxMessagesCompleted()
    {
        switch (MessageItem.ToPeer.PeerType)
        {
            case PeerType.User:
                return InboxItems.Count == 1;
            case PeerType.Chat:
                return InboxItems.Count == ChatMembers?.Count - 1;
        }

        return false;
    }
}

public class SendMessageSaga : MyInMemoryAggregateSaga<SendMessageSaga, SendMessageSagaId, SendMessageSagaLocator>,
    ISagaIsStartedBy<MessageAggregate, MessageId, OutboxMessageCreatedEvent>,
    ISagaHandles<MessageAggregate, MessageId, InboxMessageCreatedEvent>
{
    private readonly IIdGenerator _idGenerator;
    private readonly SendMessageSagaState _state = new();
    public SendMessageSaga(SendMessageSagaId id, IEventStore eventStore, IIdGenerator idGenerator) : base(id, eventStore)
    {
        _idGenerator = idGenerator;
        Register(_state);
    }

    public async Task HandleAsync(IDomainEvent<MessageAggregate, MessageId, OutboxMessageCreatedEvent> domainEvent, ISagaContext sagaContext, CancellationToken cancellationToken)
    {
        Emit(new SendMessageSagaStartedEvent(domainEvent.AggregateEvent.RequestInfo,
            domainEvent.AggregateEvent.OutboxMessageItem,
            domainEvent.AggregateEvent.MentionedUserIds,
            domainEvent.AggregateEvent.ReplyToMsgItems,
            domainEvent.AggregateEvent.ClearDraft,
            domainEvent.AggregateEvent.GroupItemCount,
            domainEvent.AggregateEvent.LinkedChannelId,
            domainEvent.AggregateEvent.ChatMembers
            ));

        await HandleSendOutboxMessageCompletedAsync();

        await CreateInboxMessageAsync(domainEvent.AggregateEvent);

        CreateMentions(domainEvent.AggregateEvent.MentionedUserIds, domainEvent.AggregateEvent.OutboxMessageItem.MessageId);
    }

    private void CreateMentions(List<long>? mentionedUserIds, int messageId)
    {
        //if (mentionedUserIds?.Count > 0)
        //{
        //    foreach (var mentionedUserId in mentionedUserIds)
        //    {
        //        var command = new CreateMentionCommand(DialogId.Create(mentionedUserId, _state.MessageItem.ToPeer),
        //            mentionedUserId, /*_state.MessageItem.ToPeer.PeerId,*/ messageId);
        //        Publish(command);
        //    }
        //}
    }

    public Task HandleAsync(IDomainEvent<MessageAggregate, MessageId, InboxMessageCreatedEvent> domainEvent, ISagaContext sagaContext, CancellationToken cancellationToken)
    {
        var item = domainEvent.AggregateEvent.InboxMessageItem;

        //var command = new AddInboxMessageIdToOutboxMessageCommand(
        //    MessageId.Create(item.SenderPeer.PeerId,
        //        domainEvent.AggregateEvent.SenderMessageId),
        //    _state.RequestInfo,
        //    item.OwnerPeer.PeerId,
        //    item.MessageId);
        //Publish(command);

        var command = new ReceiveInboxMessageCommand(
            DialogId.Create(domainEvent.AggregateEvent.InboxMessageItem.OwnerPeer.PeerId,
                domainEvent.AggregateEvent.InboxMessageItem.ToPeer),
            domainEvent.AggregateEvent.RequestInfo,
            domainEvent.AggregateEvent.InboxMessageItem.MessageId,
            domainEvent.AggregateEvent.InboxMessageItem.OwnerPeer.PeerId,
            domainEvent.AggregateEvent.InboxMessageItem.ToPeer);
        Publish(command);

        return HandleReceiveInboxMessageCompletedAsync(item);
    }

    private async Task CreateInboxMessageAsync(OutboxMessageCreatedEvent outbox)
    {
        switch (outbox.OutboxMessageItem.ToPeer.PeerType)
        {
            case PeerType.User:
                await CreateInboxMessageForUserAsync(outbox.OutboxMessageItem.ToPeer.PeerId);
                break;
            case PeerType.Chat:
                if (outbox.ChatMembers?.Count > 0)
                {
                    foreach (var chatMemberUserId in outbox.ChatMembers)
                    {
                        if (chatMemberUserId == outbox.RequestInfo.UserId)
                        {
                            continue;
                        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        CreateInboxMessageForUserAsync(chatMemberUserId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }
                }

                break;
        }
    }

    private async Task HandleReceiveInboxMessageCompletedAsync(MessageItem inboxMessageItem)
    {
        var pts = await _idGenerator.NextIdAsync(IdType.Pts, inboxMessageItem.OwnerPeer.PeerId);
        Emit(new ReceiveInboxMessageCompletedEvent2(inboxMessageItem, pts, string.Empty));

        if (_state.IsCreateInboxMessagesCompleted())
        {
            var item = _state.MessageItem;
            var command = new AddInboxItemsToOutboxMessageCommand(
                MessageId.Create(item.SenderPeer.PeerId,
                    item.MessageId),
                _state.RequestInfo,
                _state.InboxItems
                );
            Publish(command);

            await CompleteAsync();
        }
    }

    private async Task HandleSendOutboxMessageCompletedAsync()
    {
        var pts = await _idGenerator.NextIdAsync(IdType.Pts, _state.MessageItem.OwnerPeer.PeerId);
        var linkedChannelId = _state.LinkedChannelId;
        //var globalSeqNo = _state.MessageItem.ToPeer.PeerType == PeerType.Channel ? await _idGenerator.NextLongIdAsync(IdType.GlobalSeqNo) : 0;

        Emit(new SendOutboxMessageCompletedEvent2(_state.RequestInfo,
            _state.MessageItem,
            _state.MentionedUserIds,
            pts,
            _state.GroupItemCount,
            linkedChannelId,
            null//,
            //globalSeqNo
            /*_state.BotUserIds*/));

        if (_state.MessageItem.ToPeer.PeerType == PeerType.Channel)
        {
            SetChannelPts(_state.MessageItem.ToPeer.PeerId, pts, _state.MessageItem.MessageId);

            if (_state.LinkedChannelId.HasValue && _state.MessageItem.SendMessageType != SendMessageType.MessageService)
            {
                ForwardBroadcastMessageToLinkedChannel(_state.LinkedChannelId.Value, _state.MessageItem.MessageId);
            }

            // handle reply discussion message
            HandleReplyDiscussionMessage();

            await CompleteAsync();
        }
    }
    private void HandleReplyDiscussionMessage()
    {
        //if (_state.ReplyToMessageSavedFromPeerId != 0)
        //{
        //    var savedFromPeerId = _state.ReplyToMessageSavedFromPeerId;
        //    Emit(new ReplyToChannelMessageCompletedEvent2(_state.ReplyToMsgId,
        //        _state.MessageItem!.ToPeer.PeerId,
        //        _state.Pts,
        //        _state.MessageItem.MessageId,
        //        savedFromPeerId,
        //        _state.ReplyToMessageSavedFromMsgId,
        //        _state.RecentRepliers!));
        //}
    }
    private void ForwardBroadcastMessageToLinkedChannel(long linkedChannelId, int messageId)
    {
        var aggregateId = MessageId.Create(_state.MessageItem!.OwnerPeer.PeerId, messageId);
        var fromPeer = _state.MessageItem!.ToPeer;
        var toPeer = new Peer(PeerType.Channel, linkedChannelId);
        var randomBytes = new byte[8];
        Random.Shared.NextBytes(randomBytes);
        var command = new StartForwardMessageCommand(aggregateId,
            _state.RequestInfo,
            fromPeer,
            toPeer,
            new List<int> { messageId },
            new List<long> { BitConverter.ToInt64(randomBytes) },
            true,
            Guid.NewGuid()
        );
        Publish(command);
    }

    //private void ReplyToMessage(long ownerPeerId, int messageId)
    //{
    //    var aggregateId = MessageId.Create(ownerPeerId, messageId);
    //    var command = new ReplyToMessageCommand(aggregateId, _state.RequestInfo, messageId);
    //    Publish(command);
    //}

    //private void StartReplyToMessage(long ownerPeerId, Peer replierPeer, int replyToMsgId)
    //{
    //    var command =
    //        new StartReplyToMessageCommand(MessageId.Create(ownerPeerId, replyToMsgId), _state.RequestInfo, replierPeer, replyToMsgId);
    //    Publish(command);
    //}


    private void SetChannelPts(long channelId, int pts, int messageId)
    {
        var command = new SetChannelPtsCommand(ChannelId.Create(channelId), _state.MessageItem!.SenderPeer.PeerId, pts, messageId, _state.MessageItem.Date);
        Publish(command);
    }
    private async Task CreateInboxMessageForUserAsync(long inboxOwnerUserId)
    {
        var outMessageItem = _state.MessageItem!;
        var toPeer = outMessageItem.ToPeer.PeerType == PeerType.Chat ? outMessageItem.ToPeer : outMessageItem.OwnerPeer;

        int? replyToMsgId = null;
        if (!_state.ReplyToMsgItems.TryGetValue(inboxOwnerUserId, out var replyToMsgId2))
        {
            replyToMsgId = replyToMsgId2;
        }

        // Channel only create outbox message,
        // Use IdType.MessageId and IdType.ChannelMessageId will not be used
        var inboxMessageId = await _idGenerator.NextIdAsync(IdType.MessageId, inboxOwnerUserId);
        var aggregateId = MessageId.Create(inboxOwnerUserId, inboxMessageId);
        var inboxMessageItem = new MessageItem(
            new Peer(PeerType.User, inboxOwnerUserId),
            toPeer,
            outMessageItem.SenderPeer,
            inboxMessageId,
            outMessageItem.Message,
            outMessageItem.Date,
            outMessageItem.RandomId,
            false,
            outMessageItem.SendMessageType,
            outMessageItem.MessageType,
            outMessageItem.MessageSubType,
            replyToMsgId,
            outMessageItem.MessageActionData,
            outMessageItem.MessageActionType,
            outMessageItem.Entities,
            outMessageItem.Media,
            outMessageItem.GroupId,
            outMessageItem.Post,
            outMessageItem.FwdHeader,
            outMessageItem.Views,
            outMessageItem.PollId,
            outMessageItem.ReplyMarkup
        );

        var command = new CreateInboxMessageCommand(aggregateId, _state.RequestInfo, inboxMessageItem, outMessageItem.MessageId);
        Publish(command);
    }
}

public class ReceiveInboxMessageCompletedEvent2 : AggregateEvent<SendMessageSaga, SendMessageSagaId>
{
    public MessageItem MessageItem { get; }
    public int Pts { get; }
    public string? ChatTitle { get; }
    public ReceiveInboxMessageCompletedEvent2(MessageItem messageItem, int pts, string? chatTitle)
    {
        MessageItem = messageItem;
        Pts = pts;
        ChatTitle = chatTitle;
    }
}

//public class ReplyToChannelMessageCompletedEvent2 : AggregateEvent<SendMessageSaga, SendMessageSagaId>
//{
//    public ReplyToChannelMessageCompletedEvent2(int replyToMsgId,
//        long channelId,
//        int repliesPts,
//        int maxId,
//        long savedFromPeerId,
//        int savedFromMsgId,
//        IReadOnlyCollection<Peer> recentRepliers
//    )
//    {
//        ReplyToMsgId = replyToMsgId;
//        ChannelId = channelId;
//        RepliesPts = repliesPts;
//        MaxId = maxId;
//        SavedFromPeerId = savedFromPeerId;
//        SavedFromMsgId = savedFromMsgId;
//        RecentRepliers = recentRepliers;
//    }

//    public int ReplyToMsgId { get; }
//    public long ChannelId { get; }
//    public int RepliesPts { get; }
//    public int MaxId { get; }
//    public long SavedFromPeerId { get; }
//    public int SavedFromMsgId { get; }
//    public IReadOnlyCollection<Peer> RecentRepliers { get; }
//}

public class SendOutboxMessageCompletedEvent2 : RequestAggregateEvent2<SendMessageSaga, SendMessageSagaId>
{
    public MessageItem MessageItem { get; }
    public List<long>? MentionedUserIds { get; }
    public int Pts { get; }
    public int GroupItemCount { get; }
    public long? LinkedChannelId { get; }
    public IReadOnlyCollection<long>? BotUserIds { get; }
    //public long GlobalSeqNo { get; }

    public SendOutboxMessageCompletedEvent2(RequestInfo requestInfo, MessageItem messageItem,
        List<long>? mentionedUserIds,
        int pts, int groupItemCount,
        long? linkedChannelId, IReadOnlyCollection<long>? botUserIds/*, long globalSeqNo*/) : base(requestInfo)
    {
        MessageItem = messageItem;
        MentionedUserIds = mentionedUserIds;
        Pts = pts;
        GroupItemCount = groupItemCount;
        LinkedChannelId = linkedChannelId;
        BotUserIds = botUserIds;
        //GlobalSeqNo = globalSeqNo;
    }
}

public class SendMessageSagaStartedEvent : RequestAggregateEvent2<SendMessageSaga, SendMessageSagaId>
{
    public MessageItem MessageItem { get; }
    public List<long>? MentionedUserIds { get; }
    public List<ReplyToMsgItem>? ReplyToMsgItems { get; }
    public bool ClearDraft { get; }
    public int GroupItemCount { get; }
    public long? LinkedChannelId { get; }

    public List<long>? ChatMembers { get; }
    //public bool ForwardFromLinkedChannel { get; }

    public SendMessageSagaStartedEvent(RequestInfo requestInfo, MessageItem messageItem, List<long>? mentionedUserIds,
        List<ReplyToMsgItem>? replyToMsgItems,
        bool clearDraft,
        int groupItemCount,
        long? linkedChannelId,
        List<long>? chatMembers) : base(requestInfo)
    {
        MessageItem = messageItem;
        MentionedUserIds = mentionedUserIds;
        ReplyToMsgItems = replyToMsgItems;
        ClearDraft = clearDraft;
        GroupItemCount = groupItemCount;
        LinkedChannelId = linkedChannelId;
        ChatMembers = chatMembers;
        //ForwardFromLinkedChannel = forwardFromLinkedChannel;
    }
}