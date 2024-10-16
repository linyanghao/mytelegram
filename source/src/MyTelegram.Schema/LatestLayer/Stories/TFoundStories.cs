﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema.Stories;

///<summary>
/// See <a href="https://corefork.telegram.org/constructor/stories.foundStories" />
///</summary>
[TlObject(0xe2de7737)]
public sealed class TFoundStories : IFoundStories
{
    public uint ConstructorId => 0xe2de7737;
    public BitArray Flags { get; set; } = new BitArray(32);
    public int Count { get; set; }
    public TVector<MyTelegram.Schema.IFoundStory> Stories { get; set; }
    public string? NextOffset { get; set; }
    public TVector<MyTelegram.Schema.IChat> Chats { get; set; }
    public TVector<MyTelegram.Schema.IUser> Users { get; set; }

    public void ComputeFlag()
    {
        if (NextOffset != null) { Flags[0] = true; }

    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        ComputeFlag();
        writer.Write(ConstructorId);
        writer.Write(Flags);
        writer.Write(Count);
        writer.Write(Stories);
        if (Flags[0]) { writer.Write(NextOffset); }
        writer.Write(Chats);
        writer.Write(Users);
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Flags = reader.ReadBitArray();
        Count = reader.ReadInt32();
        Stories = reader.Read<TVector<MyTelegram.Schema.IFoundStory>>();
        if (Flags[0]) { NextOffset = reader.ReadString(); }
        Chats = reader.Read<TVector<MyTelegram.Schema.IChat>>();
        Users = reader.Read<TVector<MyTelegram.Schema.IUser>>();
    }
}