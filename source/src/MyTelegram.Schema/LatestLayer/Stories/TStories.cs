﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema.Stories;

///<summary>
/// List of <a href="https://corefork.telegram.org/api/stories#pinned-or-archived-stories">stories</a>
/// See <a href="https://corefork.telegram.org/constructor/stories.stories" />
///</summary>
[TlObject(0x63c3dd0a)]
public sealed class TStories : IStories
{
    public uint ConstructorId => 0x63c3dd0a;
    public BitArray Flags { get; set; } = new BitArray(32);

    ///<summary>
    /// Total number of stories that can be fetched
    ///</summary>
    public int Count { get; set; }

    ///<summary>
    /// Stories
    ///</summary>
    public TVector<MyTelegram.Schema.IStoryItem> Stories { get; set; }
    public TVector<int>? PinnedToTop { get; set; }

    ///<summary>
    /// Mentioned chats
    ///</summary>
    public TVector<MyTelegram.Schema.IChat> Chats { get; set; }

    ///<summary>
    /// Mentioned users
    ///</summary>
    public TVector<MyTelegram.Schema.IUser> Users { get; set; }

    public void ComputeFlag()
    {
        if (PinnedToTop?.Count > 0) { Flags[0] = true; }

    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        ComputeFlag();
        writer.Write(ConstructorId);
        writer.Write(Flags);
        writer.Write(Count);
        writer.Write(Stories);
        if (Flags[0]) { writer.Write(PinnedToTop); }
        writer.Write(Chats);
        writer.Write(Users);
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Flags = reader.ReadBitArray();
        Count = reader.ReadInt32();
        Stories = reader.Read<TVector<MyTelegram.Schema.IStoryItem>>();
        if (Flags[0]) { PinnedToTop = reader.Read<TVector<int>>(); }
        Chats = reader.Read<TVector<MyTelegram.Schema.IChat>>();
        Users = reader.Read<TVector<MyTelegram.Schema.IUser>>();
    }
}