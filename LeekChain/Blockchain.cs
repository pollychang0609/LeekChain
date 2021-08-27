using System;
using System.Text;
using System.Collections.Generic;
namespace LeekChain
{
    public class Blockchain
    {
        public IList<Block> Chain { set; get; }

        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.UtcNow, null, "{}");
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
            block.Hash = block.CalculateHash();
            Chain.Add(block);
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

        public Blockchain()
        {
            InitializeChain();
            CreateGenesisBlock();
            AddGenesisBlock();
        }
    }
}
