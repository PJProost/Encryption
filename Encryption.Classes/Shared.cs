using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Classes
{
    public class Shared
    {
        public static string GetIPAddress()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
                ///127.0.0.1 would be fine for this application as client and server always run on the same machine, in the same application
            }
        }
    }
}
