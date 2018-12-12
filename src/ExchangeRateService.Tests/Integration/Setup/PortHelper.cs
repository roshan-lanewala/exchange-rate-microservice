using System.Net;
using System.Net.Sockets;

namespace ExchangeRateService.Tests.Integration.Setup
{
    public static class PortHelper
    {
        public static int GetAvailablePort()
        {
            using(var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                return ((IPEndPoint) socket.LocalEndPoint).Port;
            }
        }
    }
}