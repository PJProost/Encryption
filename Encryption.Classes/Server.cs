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
                        var ascii = new ASCIIEncoding();
                        var bytes = new byte[1024];
                        var dataList = new List<byte>();
                        var i = 0;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            var dataReceived = bytes.Take(i).ToArray();
                            var dataVisual = "";
                            if (Rsa == null)
                            {
                                dataVisual = ascii.GetString(dataReceived); //ascii.GetString(bytes, 0, i);
                            } else
                            {
                                dataVisual = Convert.ToBase64String(dataReceived);
                            }
                            Console.WriteLine($"*** Data received: \"{dataVisual}\"");

                            //dataReceived.ToList().ForEach(b => dataList.Add(b));
                            //for each byte in dataReceived, add them to dataList
                            //there must be a better way?
                            dataList.AddRange(dataReceived);
                        }

                        if (dataList.Count > 0)
                        {
                            var data = dataList.ToArray();

                            //Console.WriteLine($"*** Message received: \"{ascii.GetString(data)}\"");

                            if (Rsa != null)
                            {
                                //Console.WriteLine($"*** Decryption public key: {Rsa.ToXmlString(false)}");
                                var co = new CryptObject(data);
                                var status = co.Decrypt(Rsa);
                                if (status == null)
                                {
                                    data = co.Bytes;
                                    Console.WriteLine($"*** Decrypted message received: \"{ascii.GetString(data)}\"");
                                }
                                else
                                {
                                    Console.WriteLine($"*** Message decryption failed: {status}");
                                }
                            }
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
