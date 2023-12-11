﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema.Stats;

///<summary>
/// See <a href="https://corefork.telegram.org/constructor/stats.publicForwards" />
///</summary>
[TlObject(0x93037e20)]
public sealed class TPublicForwards : IPublicForwards
{
    public uint ConstructorId => 0x93037e20;
    ///<summary>
    /// Flags, see <a href="https://corefork.telegram.org/mtproto/TL-combinators#conditional-fields">TL conditional fields</a>
    ///</summary>
    public BitArray Flags { get; set; } = new BitArray(32);

    ///<summary>
    /// &nbsp;
    ///</summary>
    public int Count { get; set; }

    ///<summary>
    /// &nbsp;
    ///</summary>
    public TVector<MyTelegram.Schema.IPublicForward> Forwards { get; set; }

    ///<summary>
    /// &nbsp;
    ///</summary>
    public string? NextOffset { get; set; }

    ///<summary>
    /// &nbsp;
    ///</summary>
    public TVector<MyTelegram.Schema.IChat> Chats { get; set; }

    ///<summary>
    /// &nbsp;
    ///</summary>
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
        writer.Write(Forwards);
        if (Flags[0]) { writer.Write(NextOffset); }
        writer.Write(Chats);
        writer.Write(Users);
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Flags = reader.ReadBitArray();
        Count = reader.ReadInt32();
        Forwards = reader.Read<TVector<MyTelegram.Schema.IPublicForward>>();
        if (Flags[0]) { NextOffset = reader.ReadString(); }
        Chats = reader.Read<TVector<MyTelegram.Schema.IChat>>();
        Users = reader.Read<TVector<MyTelegram.Schema.IUser>>();
    }
}