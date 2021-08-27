using System;
using System.Text;
using System.Collections.Generic;
namespace LeekChain
{
    public class Blockchain
    {
        public IList<Block> Chain { set; get; }
        public int Difficulty { set; get; } = 3;

        private IList<Transaction> _pendingTransactions { get; set; }

        public int Reward => 1;


        public void CreateTransaction(Transaction transaction)
        {
            _pendingTransactions.Add(transaction);
        }

        public Block CreateGenesisBlock()
        {
            var block = new Block(DateTime.UtcNow, null, _pendingTransactions);
            block.Mine(Difficulty);
            _pendingTransactions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return Chain[^1];
        }
        
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(Difficulty);
            Chain.Add(block);
        }

        public int GetBalance(string address)
        {
            int balance = 0;

            foreach(var block in Chain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if(transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }
                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];
                if(currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if(currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public void InitializeChain()
        {
            Chain = new List<Block>();
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            var block = new Block(DateTime.UtcNow, GetLatestBlock().Hash, _pendingTransactions);
            AddBlock(block);

            _pendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }

        public Blockchain()
        {
            InitializeChain();
            CreateGenesisBlock();
            AddGenesisBlock();
        }
    }
}
