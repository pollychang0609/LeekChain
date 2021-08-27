using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LeekChain
{
    public class Block
    {
        public int Index { get; set; }

        public DateTime TimeStamp { get; set; }

        public string PreviousHash { get; set; }

        public string Hash { get; set; }

        //public string Data { get; set; }
        public IList<Transaction> Transactions { get; set; }

        public int Nonce { get; set; } = 0;

        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;

            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp} {PreviousHash ?? string.Empty} {JsonSerializer.Serialize(Transactions)} {Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);


            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZero = new string('0', difficulty);
            while (Hash == null || Hash.Substring(0, difficulty) != leadingZero)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

    }
}
