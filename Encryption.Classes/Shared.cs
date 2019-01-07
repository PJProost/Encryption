using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

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

        public static string SendString(NetworkStream stream, string message, RSACryptoServiceProvider rsa = null, ResponseOption responseOption = ResponseOption.Message)
        {
            //StreamWriter could be used instead

            var ascii = new ASCIIEncoding();
            var messageBytes = ascii.GetBytes(message);

            if (rsa != null)
            {
                var co = new Crypter(messageBytes);
                co.Encrypt(rsa);
                messageBytes = co.Bytes;
            }

            Console.WriteLine($"Sending {messageBytes.Length} bytes");

            stream.Write(messageBytes, 0, messageBytes.Length);
            stream.Flush();

            if (responseOption == ResponseOption.Message)
                return ReceiveString(stream, null, ResponseOption.Response); //no encryption here
            else
                return null;
        }

        public static string ReceiveString(NetworkStream stream, RSACryptoServiceProvider rsa = null, ResponseOption responseOption = ResponseOption.Message)
        {
            //StreamReader could be used instead

            var ascii = new ASCIIEncoding();
            
            
            var buffer = new byte[128];
            var dataList = new List<byte>();
            var data = new byte[0];

            //while ((i = stream.Read(buffer, 0, buffer.Length)) > 0)
            //{ //receive data
            //    //stream.Read can would get stuck for some reason when used this way
            //}

            do
            { //receive data
                var i = 0; //number of received bytes
                i = stream.Read(buffer, 0, buffer.Length);
                var dataReceived = buffer.Take(i).ToArray();

                //to visualise the process in the console
                var dataVisual = "";
                if (rsa == null)
                {
                    dataVisual = ascii.GetString(dataReceived); //ascii.GetString(bytes, 0, i);
                }
                else
                {
                    dataVisual = Convert.ToBase64String(dataReceived);
                }
                Console.WriteLine($"*** Data received ({dataReceived.Length}B): \"{dataVisual}\"");
                dataList.AddRange(dataReceived);
            } while (stream.DataAvailable); //only available for NetworkStream, so cannot make method generic for Stream instead
            data = dataList.ToArray();

            Console.WriteLine($"*** Total data received {data.Length}B");

            if (data.Length > 0 && rsa != null)
            {
                //Console.WriteLine($"*** Decryption public key: {Rsa.ToXmlString(false)}");
                var co = new Crypter(data);
                co.Decrypt(rsa);
                data = co.Bytes;
            }

            if (responseOption == ResponseOption.Message)
                SendString(stream, "Data received", null, ResponseOption.Response); //no encryption here

            return ascii.GetString(data); //will throw when no data is received and thus, data is invalid ascii bytes
        }

        public enum ResponseOption
        {
            Response,
            Message
        }
    }
}
