﻿namespace MyTelegram.Domain.Sagas.Events;

public class CreateChannelSagaStartedEvent(
    RequestInfo requestInfo,
    string messageActionData,
    long randomId,
    bool migratedFromChat)
    : AggregateEvent<CreateChannelSaga, CreateChannelSagaId>
{
    public RequestInfo RequestInfo { get; } = requestInfo;
    public string MessageActionData { get; } = messageActionData;
    public long RandomId { get; } = randomId;
    public bool MigratedFromChat { get; } = migratedFromChat;
}