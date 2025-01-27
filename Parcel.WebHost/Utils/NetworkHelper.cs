﻿using System.Net;
using System.Net.Sockets;

namespace Parcel.WebHost.Utils
{
    public static class NetworkHelper
    {
        public static int FindFreeTcpPort()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}