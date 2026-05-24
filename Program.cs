using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var blockchain = new BlockChaineService();
            var displayService = new BlockChainDisplayService();
            var transactionService = new TransaktionServise(new WalletService());

            var AliceWallet = new WalletService().CreateWallet("Alice");
            var BobWallet = new WalletService().CreateWallet("Bob");

            while (true)
            {
                Console.WriteLine("\n1. Create Transaction");
                Console.WriteLine("2. Mine Pending Transactions");
                Console.WriteLine("3. Display Blockchain");
                Console.WriteLine("4. Display Mempool");
                Console.WriteLine("5. Transaction History");
                Console.WriteLine("6. Find Largest Transaction");
                Console.WriteLine("7. Exit");
                Console.Write("Choose: ");

                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            var tx = transactionService.CreateTransaction(AliceWallet, BobWallet.address, 10);
                            blockchain.AddTransaction(tx);
                            Console.WriteLine(" Transaction added to Mempool");
                            break;
                        case "2":
                            blockchain.MinePendingTransactions(BobWallet.address);
                            Console.WriteLine("Block mined!");
                            Console.WriteLine($"Pending: {blockchain.pendingTransactions.Count}");
                            break;
                        case "3":
                            displayService.printChain(blockchain.chain);
                            displayService.printChainValidity(blockchain.IsChainValid());
                            break;
                        case "4":
                            Console.WriteLine($"\nMempool ({blockchain.pendingTransactions.Count})", new string('-', 100));
                            foreach (var p in blockchain.pendingTransactions)
                                Console.WriteLine($"{p.from} -> {p.to}: {p.amount}");
                            break;
                        case "5":
                            Console.Write("Enter address: ");
                            string address = Console.ReadLine();
                            displayService.PrintTransactionHistory(address, blockchain.chain);
                            break;
                        case "6":
                            displayService.PrintLargestTransaction(blockchain.chain);
                            break;
                        case "7":
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
