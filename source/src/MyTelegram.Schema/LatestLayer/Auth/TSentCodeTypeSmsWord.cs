﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema.Auth;

///<summary>
/// See <a href="https://corefork.telegram.org/constructor/auth.sentCodeTypeSmsWord" />
///</summary>
[TlObject(0xa416ac81)]
public sealed class TSentCodeTypeSmsWord : ISentCodeType
{
    public uint ConstructorId => 0xa416ac81;
    public BitArray Flags { get; set; } = new BitArray(32);
    public string? Beginning { get; set; }

    public void ComputeFlag()
    {
        if (Beginning != null) { Flags[0] = true; }
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        ComputeFlag();
        writer.Write(ConstructorId);
        writer.Write(Flags);
        if (Flags[0]) { writer.Write(Beginning); }
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Flags = reader.ReadBitArray();
        if (Flags[0]) { Beginning = reader.ReadString(); }
    }
}