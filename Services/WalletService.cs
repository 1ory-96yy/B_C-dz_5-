using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class WalletService
    {
        public Wallet CreateWallet(string name)
        {
            using var ecdsa = System.Security.Cryptography.ECDsa.Create();
            var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
            var privateKey = ecdsa.ExportECPrivateKey();

            var address = new string(Convert.ToBase64String(publicKey).TakeLast(32).ToArray());
            return new Wallet(name, address, publicKey, privateKey);
        }
        public bool VerifySignature(byte[] data, byte[] signature, byte[] publicKey)
        {
            using var ecdsa = System.Security.Cryptography.ECDsa.Create();
            ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
            return ecdsa.VerifyData(data, signature, System.Security.Cryptography.HashAlgorithmName.SHA256);
        }
    }
}
