using System.Net.Sockets;
using System.Windows.Forms;
using SilkroadLauncher.Framework;
using SilkroadLauncher.Networking.Handler;
using SilkroadLauncher.SSA;

namespace SilkroadLauncher.Networking
{
    public class Session
    {
        private Socket _clientSocket;
        private byte[] _clientRecvBuffer;
        private Security _clientSecurity;
        public Session()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientRecvBuffer = new byte[4096];
            _clientSecurity = new Security();
        }

        public bool StartSession()
        {
            _clientSocket = NetHelper.ConnectWithTimeout(Global._ClientIP, Global._ClientPort, 5000);
            if(_clientSocket == null)
                return false;

            BeginRecieve();
            return true;
        }

        void BeginRecieve()
        {
            try
            {
                _clientSocket.BeginReceive(_clientRecvBuffer, 0, _clientRecvBuffer.Length, SocketFlags.None,
                    (iar) =>
                    {
                        try
                        {
                            int recvCount = _clientSocket.EndReceive(iar);

                            if (recvCount == 0)
                                NetHelper.CloseSocket(ref _clientSocket);

                            if (recvCount > 0)
                            {

                                _clientSecurity.Recv(_clientRecvBuffer, 0, recvCount);

                                PacketManager.ExecutePacket(_clientSecurity.TransferIncoming(), this);

                                SendServer();
                            }

                        }
                        catch
                        {
                            NetHelper.CloseSocket(ref _clientSocket);
                        }
                        BeginRecieve();
                    }, null);
            }
            catch
            {
                NetHelper.CloseSocket(ref _clientSocket);
            }
        }

        void SendServer()
        {
            try
            {
                var buffers = _clientSecurity.TransferOutgoing();
                if (buffers != null)
                {
                    foreach (var kvp in buffers)
                    {
                        _clientSocket.BeginSend(kvp.Key.Buffer, kvp.Key.Offset, kvp.Key.Size, SocketFlags.None,
                            (iar) =>
                            {
                                try
                                {
                                    _clientSocket.EndSend(iar);
                                }
                                catch
                                {
                                    NetHelper.CloseSocket(ref _clientSocket);
                                }
                            }, null);
                    }
                }
            }
            catch
            {
                NetHelper.CloseSocket(ref _clientSocket);
            }
        }
        public void SendPacket(Packet packet)
        {
            _clientSecurity.Send(packet);
        }

    }
}
