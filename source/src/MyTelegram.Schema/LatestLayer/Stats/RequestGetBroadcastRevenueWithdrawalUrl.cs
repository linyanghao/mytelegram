﻿// <auto-generated/>
// ReSharper disable All

namespace MyTelegram.Schema.Stats;

///<summary>
/// See <a href="https://corefork.telegram.org/method/stats.getBroadcastRevenueWithdrawalUrl" />
///</summary>
[TlObject(0x2a65ef73)]
public sealed class RequestGetBroadcastRevenueWithdrawalUrl : IRequest<MyTelegram.Schema.Stats.IBroadcastRevenueWithdrawalUrl>
{
    public uint ConstructorId => 0x2a65ef73;
    public MyTelegram.Schema.IInputChannel Channel { get; set; }
    public MyTelegram.Schema.IInputCheckPasswordSRP Password { get; set; }

    public void ComputeFlag()
    {

    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        ComputeFlag();
        writer.Write(ConstructorId);
        writer.Write(Channel);
        writer.Write(Password);
    }

    public void Deserialize(ref SequenceReader<byte> reader)
    {
        Channel = reader.Read<MyTelegram.Schema.IInputChannel>();
        Password = reader.Read<MyTelegram.Schema.IInputCheckPasswordSRP>();
    }
}
