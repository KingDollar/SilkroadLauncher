namespace SilkroadLauncher.Framework
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    static class NetHelper
    {
        public static void CloseSocket(ref Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch { }
            finally { socket = null; }
        }

        public static IPAddress ParseOrResolveAddress(string address)
        {
            IPAddress ip = null;

            bool isNormalIp = IPAddress.TryParse(address, out ip);
            if (isNormalIp)
                return ip;

            try
            {
                var hostEntry = Dns.GetHostEntry(address);
                ip = hostEntry.AddressList.FirstOrDefault();
            }
            catch { }

            return ip;
        }

        public static Socket ConnectWithTimeout(IPEndPoint endPoint, int timeout)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IAsyncResult iar = socket.BeginConnect(endPoint, null, null);
                if (iar.AsyncWaitHandle.WaitOne(timeout))
                {
                    socket.EndConnect(iar);
                    return socket;
                }
            }
            catch { }

            CloseSocket(ref socket);

            return socket;
        }


        public static Socket ConnectWithTimeout(string address, int port, int timeout)
        {
            IPAddress ip = ParseOrResolveAddress(address);
            if (ip == null)
                return null;

            return ConnectWithTimeout(new IPEndPoint(ip, port), timeout);
        }

        public static string ReadURLIP(string url)=> Dns.GetHostAddresses(new Uri(url).Host)[0].ToString();

    }
}
