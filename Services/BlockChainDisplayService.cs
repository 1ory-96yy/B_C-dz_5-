using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class BlockChainDisplayService
    {
        public void printChain(List<Models.Block> chain)
        {
            foreach (var block in chain)
            {
                Console.WriteLine($"Index: {block.index}");
                Console.WriteLine($"Timestamp: {block.timestamp}");
                Console.WriteLine($"Hash: {block.hash}");
                Console.WriteLine($"Nonce: {block.nonce}");
                Console.WriteLine($"Mining Difficulty: {block.Difficulty}");
                Console.WriteLine($"Previous Hash: {block.previousHash}");
                Console.WriteLine(new string('-', 20));
                printTransaction(block.transactions);
            }

        }

        public void printChainValidity(bool isValid)
        {
            if (isValid)
                Console.WriteLine("The blockchain is valid.");
            else
                Console.WriteLine("The blockchain is invalid.");
        }


        public void printTransaction(List<transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"From: {transaction.from}, To: {transaction.to}, Amount: {transaction.amount}");
                Console.WriteLine(new string('-', 20));

            }
        }

        public void PrintTransactionHistory(string address, List<Block> chain)
        {
            var foundTransactions = new List<(int blockIndex, transaction tx)>();

            foreach (var block in chain.Skip(1))
            {
                foreach (var tx in block.transactions)
                {
                    if (tx.from.Equals(address, StringComparison.OrdinalIgnoreCase) || 
                        tx.to.Equals(address, StringComparison.OrdinalIgnoreCase))
                    {
                        foundTransactions.Add((block.index, tx));
                    }
                }
            }

            if (foundTransactions.Count == 0)
            {
                Console.WriteLine($"\nNo transactions found for address: {address}");
                return;
            }

            Console.WriteLine($"\n╔══════════════════════════════════════════════╗");
            Console.WriteLine($"║  Transaction history for {address}  ║ ");
            Console.WriteLine($"╚══════════════════════════════════════════════╝\n");

            foreach (var (blockIndex, tx) in foundTransactions)
            {
                Console.WriteLine($"Block #{blockIndex}");
                if (tx.from.Equals(address, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Sent to {tx.to}: {tx.amount} coins");
                }
                else
                {
                    Console.WriteLine($"Received from {tx.from}: {tx.amount} coins");
                }
                Console.WriteLine($"Time: {tx.timestamp}");
                Console.WriteLine();
            }
        }

        public void PrintLargestTransaction(List<Block> chain)
        {
            var largestTx = chain
                .Skip(1)
                .SelectMany((block, _) => block.transactions.Select(tx => (blockIndex: block.index, transaction: tx)))
                .OrderByDescending(x => x.transaction.amount)
                .FirstOrDefault();

            if (largestTx.transaction == null)
            {
                Console.WriteLine("No transactions found in the blockchain.");
                return;
            }

            Console.WriteLine($"\nLargest transaction in the network:");
            Console.WriteLine($"   Block #{largestTx.blockIndex}");
            Console.WriteLine($"   {largestTx.transaction.from} → {largestTx.transaction.to}");
            Console.WriteLine($"   Amount: {largestTx.transaction.amount} coins\n");
        }

    }
}
