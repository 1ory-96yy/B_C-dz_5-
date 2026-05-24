using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Wallet
    {
        public string name { get;  }
        public string address { get;}
        public byte[] publicKey { get; }
        public byte[] privateKey { get; }
        
        public Wallet(string name, string address, byte[] publicKey, byte[] privateKey)
        {
            this.name = name;
            this.address = address;
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        public byte[] Sign(byte[] data)
        {
            using var ecdsa = System.Security.Cryptography.ECDsa.Create();
            ecdsa.ImportECPrivateKey(privateKey, out _);
            return ecdsa.SignData(data, System.Security.Cryptography.HashAlgorithmName.SHA256);
        }
    }
}
