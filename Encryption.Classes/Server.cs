using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Encryption.Classes;
using System.Security.Cryptography;

namespace Encryption.Classes
{
    public class Server
    {
        public Server(string listenIP, int port)
        {
            ListenIP = listenIP;
            Port = port;
        }

        public void WaitForMessage()
        {
            Console.WriteLine("Server thread started");

            var listener = new TcpListener(IPAddress.Parse(ListenIP), Port);
            listener.Start();
            Console.WriteLine($"Listening on IP {ListenIP}:{Port}");

            do
            {
                using (var client = listener.AcceptTcpClient())
                //this causes the thread to wait for a connection
                {
                    using (var stream = client.GetStream())
                    {
                        try
                        {
                            Console.WriteLine($"*** Message received: \"{Shared.ReceiveString(stream, Rsa)}\"");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"*** Message receiving failed: {e.Message}");
                        }
                    }
                }
            } while (true); //loop

            //listener.Stop(); will never be reached
        }

        public string ListenIP { get; set; }
        public int Port { get; set; }

        public RSACryptoServiceProvider Rsa { get; set; }
    }
}
