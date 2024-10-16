﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema;

///<summary>
/// See <a href="https://corefork.telegram.org/constructor/inputKeyboardButtonRequestPeer" />
///</summary>
[TlObject(0xc9662d05)]
public sealed class TInputKeyboardButtonRequestPeer : IKeyboardButton
{
    public uint ConstructorId => 0xc9662d05;
    public BitArray Flags { get; set; } = new BitArray(32);
    public bool NameRequested { get; set; }
    public bool UsernameRequested { get; set; }
    public bool PhotoRequested { get; set; }
    public string Text { get; set; }
    public int ButtonId { get; set; }
    public MyTelegram.Schema.IRequestPeerType PeerType { get; set; }
    public int MaxQuantity { get; set; }

    public void ComputeFlag()
    {
        if (NameRequested) { Flags[0] = true; }
        if (UsernameRequested) { Flags[1] = true; }
        if (PhotoRequested) { Flags[2] = true; }

    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        ComputeFlag();
        writer.Write(ConstructorId);
        writer.Write(Flags);
        writer.Write(Text);
        writer.Write(ButtonId);
        writer.Write(PeerType);
        writer.Write(MaxQuantity);
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Flags = reader.ReadBitArray();
        if (Flags[0]) { NameRequested = true; }
        if (Flags[1]) { UsernameRequested = true; }
        if (Flags[2]) { PhotoRequested = true; }
        Text = reader.ReadString();
        ButtonId = reader.ReadInt32();
        PeerType = reader.Read<MyTelegram.Schema.IRequestPeerType>();
        MaxQuantity = reader.ReadInt32();
    }
}