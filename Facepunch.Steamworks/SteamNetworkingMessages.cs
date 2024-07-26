using System;
using System.Runtime.InteropServices;
using Steamworks.Data;

namespace Steamworks {
    /// <summary>
    /// Class for utilizing the Steam Network API.
    /// </summary>
    public class SteamNetworkingMessages : SteamSharedClass<SteamNetworkingMessages> {
        internal static ISteamNetworkingMessages Internal => Interface as ISteamNetworkingMessages;

        internal override bool InitializeInterface(bool server) {
            SetInterface(server, new ISteamNetworkingMessages(server));
            if (Interface.Self == IntPtr.Zero) return false;

            InstallEvents(server);

            return true;
        }

        internal static void InstallEvents(bool server) {
            Dispatch.Install<SteamNetworkingMessagesSessionRequest_t>(x => OnSteamNetworkingMessagesSessionRequest?.Invoke(x.IdentityRemote), server);
            Dispatch.Install<SteamNetworkingMessagesSessionFailed_t>(x => OnSteamNetworkingMessagesSessionFailed?.Invoke(x.Info), server);
        }

        public static event Action<NetIdentity> OnSteamNetworkingMessagesSessionRequest;
        public static event Action<ConnectionInfo> OnSteamNetworkingMessagesSessionFailed;
        public static event Action<Connection, NetIdentity, IntPtr, int, long, long, int> OnReceiveMessage;

        public static Result SendMessageToUser(NetIdentity identityRemote, IntPtr pubData, uint cubData, SendType nSendFlags, int nRemoteChannel) => Internal.SendMessageToUser(ref identityRemote, pubData, cubData, (int)nSendFlags, nRemoteChannel);
        public unsafe static int ReceiveMessagesOnChannel(int nLocalChannel, IntPtr ppOutMessages, int nMaxMessages) => UnsafeReceiveMessagesOnChannel(nLocalChannel, ppOutMessages, nMaxMessages);

        internal unsafe static int UnsafeReceiveMessagesOnChannel(int nLocalChannel, IntPtr ppOutMessages, int nMaxMessages) {
            int numberOfMessages = Internal.ReceiveMessagesOnChannel(nLocalChannel, ppOutMessages, nMaxMessages);
            for (int i = 0; i < numberOfMessages; i++) {
                IntPtr messagePtr = Marshal.ReadIntPtr(ppOutMessages, i * IntPtr.Size);
                NetMsg message = Marshal.PtrToStructure<NetMsg>(messagePtr);
                OnReceiveMessage?.Invoke(message.Connection, message.Identity, message.DataPtr, message.DataSize, message.RecvTime, message.MessageNumber, message.Channel);
                NetMsg.InternalRelease((NetMsg*)messagePtr);

            }
            return numberOfMessages;
        }

        public static bool AcceptSessionWithUser(NetIdentity identityRemote) => Internal.AcceptSessionWithUser(ref identityRemote);
        public static bool CloseSessionWithUser(NetIdentity identityRemote) => Internal.CloseSessionWithUser(ref identityRemote);
        public static bool CloseChannelWithUser(NetIdentity identityRemote, int nLocalChannel) => Internal.CloseChannelWithUser(ref identityRemote, nLocalChannel);
        public static ConnectionState GetSessionConnectionInfo(NetIdentity identityRemote, ref ConnectionInfo pConnectionInfo, ref ConnectionStatus pQuickStatus) => Internal.GetSessionConnectionInfo(ref identityRemote, ref pConnectionInfo, ref pQuickStatus);

    }
}