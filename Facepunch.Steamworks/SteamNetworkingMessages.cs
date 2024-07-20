using System;
using System.Runtime.InteropServices;
using Steamworks.Data;

namespace Steamworks
{
	/// <summary>
	/// Class for utilizing the Steam Network API.
	/// </summary>
	public class SteamNetworkingMessages : SteamSharedClass<SteamNetworkingMessages>
	{
		internal static ISteamNetworkingMessages Internal => Interface as ISteamNetworkingMessages;

		internal override bool InitializeInterface( bool server )
		{
			SetInterface( server, new ISteamNetworkingMessages( server ) );
			if ( Interface.Self == IntPtr.Zero ) return false;

			InstallEvents( server );

			return true;
		}

		internal static void InstallEvents( bool server )
		{
      Dispatch.Install<SteamNetworkingMessagesSessionRequest_t>( x => OnSteamNetworkingMessagesSessionRequest?.Invoke(x.IdentityRemote), server);
      Dispatch.Install<SteamNetworkingMessagesSessionFailed_t>( x => OnSteamNetworkingMessagesSessionFailed?.Invoke(x.Info), server);
		}

    public static Action<NetIdentity> OnSteamNetworkingMessagesSessionRequest;
    public static Action<ConnectionInfo> OnSteamNetworkingMessagesSessionFailed;

    public static Result SendMessageToUser( ref NetIdentity identityRemote, [In,Out] IntPtr[] pubData, uint cubData, int nSendFlags, int nRemoteChannel ) => Internal.SendMessageToUser(ref identityRemote, pubData, cubData, nSendFlags, nRemoteChannel);
    public static int ReceiveMessagesOnChannel( int nLocalChannel, IntPtr ppOutMessages, int nMaxMessages ) => Internal.ReceiveMessagesOnChannel(nLocalChannel, ppOutMessages, nMaxMessages);
    public static bool AcceptSessionWithUser( ref NetIdentity identityRemote ) => Internal.AcceptSessionWithUser(ref identityRemote);
    public static bool CloseSessionWithUser( ref NetIdentity identityRemote ) => Internal.CloseSessionWithUser(ref identityRemote);
    public static bool CloseChannelWithUser( ref NetIdentity identityRemote, int nLocalChannel ) => Internal.CloseChannelWithUser(ref identityRemote, nLocalChannel);
    public static ConnectionState GetSessionConnectionInfo( ref NetIdentity identityRemote, ref ConnectionInfo pConnectionInfo, ref ConnectionStatus pQuickStatus ) => Internal.GetSessionConnectionInfo(ref identityRemote, ref pConnectionInfo, ref pQuickStatus);

  }
}