﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema.Messages;

///<summary>
/// Change the set of <a href="https://corefork.telegram.org/api/reactions">message reactions »</a> that can be used in a certain group, supergroup or channel
/// <para>Possible errors</para>
/// Code Type Description
/// 400 CHAT_ADMIN_REQUIRED You must be an admin in this chat to do this.
/// 400 CHAT_NOT_MODIFIED No changes were made to chat information because the new information you passed is identical to the current information.
/// 400 PEER_ID_INVALID The provided peer id is invalid.
/// See <a href="https://corefork.telegram.org/method/messages.setChatAvailableReactions" />
///</summary>
[TlObject(0x5a150bd4)]
public sealed class RequestSetChatAvailableReactions : IRequest<MyTelegram.Schema.IUpdates>
{
    public uint ConstructorId => 0x5a150bd4;
    public BitArray Flags { get; set; } = new BitArray(32);

    ///<summary>
    /// Group where to apply changes
    /// See <a href="https://corefork.telegram.org/type/InputPeer" />
    ///</summary>
    public MyTelegram.Schema.IInputPeer Peer { get; set; }

    ///<summary>
    /// Allowed reaction emojis
    /// See <a href="https://corefork.telegram.org/type/ChatReactions" />
    ///</summary>
    public MyTelegram.Schema.IChatReactions AvailableReactions { get; set; }
    public int? ReactionsLimit { get; set; }

    public void ComputeFlag()
    {
        if (/*ReactionsLimit != 0 && */ReactionsLimit.HasValue) { Flags[0] = true; }
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        ComputeFlag();
        writer.Write(ConstructorId);
        writer.Write(Flags);
        writer.Write(Peer);
        writer.Write(AvailableReactions);
        if (Flags[0]) { writer.Write(ReactionsLimit.Value); }
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Flags = reader.ReadBitArray();
        Peer = reader.Read<MyTelegram.Schema.IInputPeer>();
        AvailableReactions = reader.Read<MyTelegram.Schema.IChatReactions>();
        if (Flags[0]) { ReactionsLimit = reader.ReadInt32(); }
    }
}
