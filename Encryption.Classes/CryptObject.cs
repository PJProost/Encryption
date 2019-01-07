using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Classes
{
    public class CryptObject
    {
        public CryptObject(byte[] b)
        {
            Bytes = b;
        }
        public byte[] Bytes { get; set; }
        public void Encrypt(RSACryptoServiceProvider rsa)
        {
            Bytes = rsa.Encrypt(Bytes, true);
        }
        public string Decrypt(RSACryptoServiceProvider rsa)
        {
            try
            {
                Bytes = rsa.Decrypt(Bytes, true);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
