using System;
using System.Threading;
using System.Security.Cryptography;
using Encryption.Classes;

namespace Encryption.Messaging
{
    class Program
    {
        private static void Main(string[] args)
        {
            var receiverRsa = new RSACryptoServiceProvider();
            receiverRsa.FromXmlString("<RSAKeyValue><Modulus>pjz3B7yyuN7u5DUMxxsVeJ6DbRyMyNGpqMFgQXtSCDAAIq3iIiV7RR22tUlq3F875NNYmsZdG91zqtg/1ja0Nb4qByJR3nMcZxUz8E5C3idXre9L9MyHQF/s8cYiuRq6ed7Ro5DNHJdNU6NUW+bnUIFwuQLhDRjcFDHTu/d3Ask=</Modulus><Exponent>AQAB</Exponent><P>wqE4pxBT5fe9fRGHaP3Dgcwm3QxD9appfURQPO9bI4Yd7rtt8grKH31yZvw2BnL3Ct52VDVEdgZnqn4lm+Rz4w==</P><Q>2qfu4b9N6DMLKskM+eMUoplYs0ySItB02c3JHm686Y8pzOhb5Vl1M25yzsXx5IJ/p7FxCwtvkX4qPr38VHOmYw==</Q><DP>X/cScf1xAMEIo3RTKgeFsKgyuWdk0uq1nNhkH8d9TqTAeYfdDC0ZwDEgiXruQHvLJ4bNHXQuT2uVDdGpRZZ9NQ==</DP><DQ>scg1PKu1BoTqIYGS4WK3FnWkXzR05YWkXKsrSWk0hJp4nDiY72PLHWRCSMk9IlTQwmJNzXMg5aU1aApFLc1SjQ==</DQ><InverseQ>ffXOpOpEjdXAQ/Zg3EIj59QiXjKaxqdmjRcSqZS0M5IXj5fifwSTc5TcmxOf+IJaE8LCrRsGcarC5R1r1V0DkQ==</InverseQ><D>WaBo94zvNulLF1LazsZ1bxDXfw5zgRo5RLjtsqBQfAWVLR1e3FYk/gClL1yj9qiJ3DdugBQOwyVEZYot8MqRocFQPH3BT/0GIAqIsRxZPzB7bjDXF2ivfHOVujTmSobW2A9LeyWRuipR9X2x48EkhfAaGi0EGMFW0vFcezj3mxk=</D></RSAKeyValue>");
            //to be shared between client and server for encryption en decryption purposes
            //in a real application it would be loaded from file or the machine store
            //in the real world the sender would sign with their private and encrypt with the receivers public
            //encrypting: to ensure the identity of the recipient
            //signing: to ensure the identity of the sender
            //no signing was implemented in this exercise

            var server = new Server
            {
                Rsa = receiverRsa, //receive data without decrypting by commenting this line
                ListenIP = Shared.GetIPAddress(),
                Port = 1337
            };
            var serverThread = new Thread(server.WaitForMessage)
            {
                IsBackground = true
            };
            serverThread.Start();
            Thread.Sleep(250); //wait for thread to start

            do
            {
                Console.WriteLine("Enter message or \"exit\" to end:");
                var message = Console.ReadLine();
                if (message == "exit") {
                    break;
                }
                var client = new Client
                {
                    Rsa = receiverRsa, //send plain text data without encryption by commenting this line
                    ServerIP = Shared.GetIPAddress(),
                    Port = 1337
                };
                client.SendMessage(message);

                Thread.Sleep(250);
            } while (true); //loop

            Console.WriteLine("Closing program...");
        }
    }
}
