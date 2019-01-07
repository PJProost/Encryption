using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Encryption.Classes
{
    public class Client
    {
        public Client(string serverIP, int port)
        {
            ServerIP = serverIP;
            Port = port;
        }

        public void SendMessage(string message)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    client.Connect(IPAddress.Parse(ServerIP), Port);

                    //Console.WriteLine($"Encryption public key: {Rsa.ToXmlString(false)}");
                    Console.WriteLine($"Response: {Shared.SendString(client.GetStream(), message, Rsa)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Send of message failed: {e.Message}");
                }
            }
        }

        public string ServerIP { get; set; }
        public int Port { get; set; }

        public RSACryptoServiceProvider Rsa { get; set; }
    }
}
