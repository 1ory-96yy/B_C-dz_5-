using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
    internal class HashingService
    {
        public string ComputeHash(Models.Block block)
        {
            string transactionsData = string.Concat(block.transactions.Select(t => t.ToRowString()));
            var input = $"{block.index}{block.timestamp}{transactionsData}{block.previousHash}{block.nonce}";

            return ComputeSha256(input);
        }
        public string ComputeSha256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}
