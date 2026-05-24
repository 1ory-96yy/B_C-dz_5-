using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class BlockChaineService
    {
        public List<Block> chain { get; set; }

        public List<transaction> pendingTransactions { get; set; }

        public int miningReward { get; set; } = 50;
        public int Difficulty { get; set; } = 3;
        public int MaxTransactionsPerBlock { get; set; } = 10;
        public int MaxTransactionsPerAddress { get; set; } = 2;
        private readonly HashingService hashingService;
        private readonly int _adjustmentInterval = 5;
        private readonly int _blockGenerationInterval = 10;

        private readonly MiningService _miningService;

        private readonly TransaktionServise _transaktionServise;

        public BlockChaineService()
        {
            this._transaktionServise = new TransaktionServise(new WalletService());
            this.chain = new List<Block>();
            this.hashingService = new HashingService();
            this._miningService = new MiningService();
            this.CreateGenesisBlock();
            pendingTransactions = new List<transaction>();
        }

        private void CreateGenesisBlock()
        {
            var genesisBlock = new Block(0, new List<transaction>(), "0");
            genesisBlock.hash = this.hashingService.ComputeHash(genesisBlock);
            this.chain.Add(genesisBlock);
        }

        //public void AddBlock(List<transaction> transactions)
        //{
        //    foreach (var transaction in transactions) {
        //        if (!_transaktionServise.ValidateTransaction(transaction).isValid)
        //        {
        //            throw new Exception($"Invalid transaction from {transaction.from} to {transaction.to} for amount {transaction.amount}");
        //        }
        //    }
        //    var previousBlock = this.chain.Last();
        //    var newBlock = new Block(previousBlock.index + 1, transactions, previousBlock.hash);
        //    newBlock.Difficulty = this.Difficulty;
        //    _miningService.MineBlock(newBlock, Difficulty);

        //    this.chain.Add(newBlock);
        //    if (newBlock.index % _adjustmentInterval == 0)
        //    {
        //        AdjustDifficulty();
        //    }
        //}
        public void MinePendingTransactions(string minerAddress)
        {
            var rewardTransaction = new transaction("coinbase", minerAddress, miningReward);

            int maxTransactionsToInclude = MaxTransactionsPerBlock - 1;
            var transactionsToMine = pendingTransactions.Take(maxTransactionsToInclude).ToList();

            var previousBlock = this.chain.Last();
            var totalTransactions = new List<transaction>(transactionsToMine) { rewardTransaction };
            var newBlock = new Block(previousBlock.index + 1, totalTransactions, previousBlock.hash);
            newBlock.Difficulty = this.Difficulty;
            _miningService.MineBlock(newBlock, Difficulty);
            chain.Add(newBlock);

            foreach (var tx in transactionsToMine)
            {
                pendingTransactions.Remove(tx);
            }

            if (newBlock.index % _adjustmentInterval == 0)
            {
                AdjustDifficulty();
            }
        }


        public void AddTransaction(transaction transaction)
        {
            if (!_transaktionServise.ValidateTransaction(transaction).isValid)
            {
                throw new Exception($"Invalid transaction from {transaction.from} to {transaction.to} for amount {transaction.amount}");
            }
            if (pendingTransactions.Any(tx => tx.id == transaction.id))
            {
                throw new Exception($"Transaction with id {transaction.id} already exists in Mempool.");
            }

            foreach (var block in chain)
            {
                if (block.transactions.Any(tx => tx.id == transaction.id))
                {
                    throw new Exception($"Transaction with id {transaction.id} already confirmed in blockchain.");
                }
            }

            int transactionsFromSender = pendingTransactions.Count(tx => tx.from == transaction.from);
            if (transactionsFromSender >= MaxTransactionsPerAddress)
            {
                throw new Exception($"Address {transaction.from} has reached maximum pending transactions limit ({MaxTransactionsPerAddress}). Please wait for previous transactions to be confirmed.");
            }

            pendingTransactions.Add(transaction);
        }


        private void AdjustDifficulty()
        {
            var recentBlocks = this.chain.Where(b => b.index > 0).TakeLast(_adjustmentInterval).ToList();
            if (recentBlocks.Count == 0)
                return;
            double averageMiningTime = recentBlocks.Average(x => (x.timestamp - this.chain[x.index - 1].timestamp).TotalSeconds);
            if (averageMiningTime < _blockGenerationInterval / 2)
            {
                Difficulty++;
            }
            else if (averageMiningTime > _blockGenerationInterval * 2 && Difficulty > 1)
            {
                Difficulty = Math.Max(1, Difficulty - 1);
            }
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < this.chain.Count; i++)
            {
                var currentBlock = this.chain[i];
                var previousBlock = this.chain[i - 1];
                if (currentBlock.hash != this.hashingService.ComputeHash(currentBlock))
                    return false;
                if (currentBlock.previousHash != previousBlock.hash)
                    return false;
                if (!currentBlock.hash.StartsWith(new string('0', currentBlock.Difficulty)))
                    return false;
            }
            return true;
        }
    }
}
