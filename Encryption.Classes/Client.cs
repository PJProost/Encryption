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
        public void SendMessage(string message)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    client.Connect(IPAddress.Parse(ServerIP), Port);
                    var stream = client.GetStream();
                    var ascii = new ASCIIEncoding();
                    var messageBytes = ascii.GetBytes(message);

                    if (Rsa != null)
                    {
                        //Console.WriteLine($"Encryption public key: {Rsa.ToXmlString(false)}");
                        var co = new CryptObject(messageBytes);
                        co.Encrypt(Rsa);
                        messageBytes = co.Bytes;
                    }

                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Send of message failed: {e.Message}");
                }
            }
        }

        public int Port { get; set; }
        public string ServerIP { get; set; }

        public RSACryptoServiceProvider Rsa { get; set; }
    }
}
