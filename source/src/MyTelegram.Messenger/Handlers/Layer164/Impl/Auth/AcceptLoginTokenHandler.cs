﻿// ReSharper disable All

namespace MyTelegram.Handlers.Auth;

///<summary>
/// Accept QR code login token, logging in the app that generated it.Returns info about the new session.For more info, see <a href="https://corefork.telegram.org/api/qr-login">login via QR code</a>.
/// <para>Possible errors</para>
/// Code Type Description
/// 400 AUTH_TOKEN_ALREADY_ACCEPTED The specified auth token was already accepted.
/// 400 AUTH_TOKEN_EXCEPTION An error occurred while importing the auth token.
/// 400 AUTH_TOKEN_EXPIRED The authorization token has expired.
/// 400 AUTH_TOKEN_INVALIDX The specified auth token is invalid.
/// See <a href="https://corefork.telegram.org/method/auth.acceptLoginToken" />
///</summary>
internal sealed class AcceptLoginTokenHandler : RpcResultObjectHandler<MyTelegram.Schema.Auth.RequestAcceptLoginToken, MyTelegram.Schema.IAuthorization>,
    Auth.IAcceptLoginTokenHandler
{
    protected override Task<MyTelegram.Schema.IAuthorization> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Auth.RequestAcceptLoginToken obj)
    {
        throw new NotImplementedException();
    }
}
