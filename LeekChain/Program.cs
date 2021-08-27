using System;
using System.Text.Json;

namespace LeekChain
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain leakCoin = new Blockchain();

            leakCoin.AddBlock(new Block(DateTime.UtcNow, null, "a quick brown fox"));
            leakCoin.AddBlock(new Block(DateTime.UtcNow, null, "jumped over"));
            leakCoin.AddBlock(new Block(DateTime.UtcNow, null, "a lazy dog"));

            var json = JsonSerializer.Serialize(leakCoin, new JsonSerializerOptions() { WriteIndented = true });

            Console.WriteLine(json);
            Console.ReadLine();
        }
    }

}
