using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
    public class TransaktionServise
    {
        private readonly WalletService _walletService;
        public TransaktionServise(WalletService walletService)
        {
            _walletService = walletService;
        }
        public transaction CreateTransaction(Wallet sender, string to, double amount)
        {
            var newTransaction = new transaction(sender.address, to, amount);
            newTransaction.SenderPublicKey = sender.publicKey;
            newTransaction.Signature = sender.Sign(newTransaction.GetDataForSigning());

            if (ValidateTransaction(newTransaction).isValid)
            {
                return newTransaction;
            }
            else
            {
                throw new ArgumentException("Invalid transaction data.");
            }
        }


        public(bool isValid, string errorMessage) ValidateTransaction(transaction transaction)
        {
            if (transaction.from == "coinbase")
            {
                return (true, string.Empty);
            }
            if (string.IsNullOrEmpty(transaction.from))
            {
                return (false, "Sender address is required.");
            }
            if (string.IsNullOrEmpty(transaction.to))
            {
                return (false, "Recipient address is required.");
            }
            if (transaction.amount <= 0)
            {
                return (false, "Amount must be greater than zero.");
            }

            bool isSignatureValid = _walletService.VerifySignature( transaction.GetDataForSigning(),transaction.Signature, transaction.SenderPublicKey);
            if (!isSignatureValid)
            {
                return (false, "Invalid signature.");
            }
            return (true, string.Empty);
        }
    }
}
