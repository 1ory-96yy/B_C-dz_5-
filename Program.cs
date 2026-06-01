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

            var CarolWallet = new WalletService().CreateWallet("Carol");
            var DaveWallet = new WalletService().CreateWallet("Dave");
            var EveWallet = new WalletService().CreateWallet("Eve");

            try
            {
                var tx1 = transactionService.CreateTransaction(AliceWallet, BobWallet.address, 10);
                var tx2 = transactionService.CreateTransaction(BobWallet, CarolWallet.address, 5);
                blockchain.AddTransaction(tx1);
                blockchain.AddTransaction(tx2);
                blockchain.MinePendingTransactions(BobWallet.address);

                var tx3 = transactionService.CreateTransaction(CarolWallet, AliceWallet.address, 20);
                var tx4 = transactionService.CreateTransaction(DaveWallet, AliceWallet.address, 1000);
                blockchain.AddTransaction(tx3);
                blockchain.AddTransaction(tx4);
                blockchain.MinePendingTransactions(EveWallet.address);

                var tx5 = transactionService.CreateTransaction(AliceWallet, EveWallet.address, 2.5);
                var tx6 = transactionService.CreateTransaction(EveWallet, BobWallet.address, 1.25);
                blockchain.AddTransaction(tx5);
                blockchain.AddTransaction(tx6);
                blockchain.MinePendingTransactions(AliceWallet.address);

                var tx7 = transactionService.CreateTransaction(BobWallet, DaveWallet.address, 50);
                var tx8 = transactionService.CreateTransaction(DaveWallet, CarolWallet.address, 250);
                blockchain.AddTransaction(tx7);
                blockchain.AddTransaction(tx8);
                blockchain.MinePendingTransactions(DaveWallet.address);

                Console.WriteLine("\n--- Demo blockchain created with 4 blocks (plus genesis) ---\n");
                displayService.printChain(blockchain.chain);
                displayService.PrintTransactionHistory(AliceWallet.address, blockchain.chain);
                displayService.PrintLargestTransaction(blockchain.chain);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Demo setup warning: {ex.Message}");
                Console.ResetColor();
            }

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
