using System;
using System.Diagnostics;
using System.Text.Json;

namespace LeekChain
{
    class Program
    {
        public static int Port { get; set; }
        public static string Address { get; set; }

        public static Blockchain CurrentChain { get; set; }

        static void Main(string[] args)
        {
            Blockchain leakCoin = new Blockchain();
            leakCoin.CreateTransaction(new Transaction("Boss", "Diaos", 996));
            leakCoin.ProcessPendingTransactions("Edi");
            leakCoin.CreateTransaction(new Transaction("Diaos", "Boss", 100));
            leakCoin.CreateTransaction(new Transaction("Diaos", "Boss", 200));
            leakCoin.ProcessPendingTransactions("Edi");
            leakCoin.CreateTransaction(new Transaction("Diaos", "Boss", 300));
            leakCoin.ProcessPendingTransactions("Edi");

            Console.WriteLine($"Boss: {leakCoin.GetBalance("Boss")}");
            Console.WriteLine($"Diaos: {leakCoin.GetBalance("Diaos")}");
            Console.WriteLine($"Edi: {leakCoin.GetBalance("Edi")}");

            var json = JsonSerializer.Serialize(leakCoin, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(json);
            Console.WriteLine($"Is Chain Valid: {leakCoin.IsValid()}");
            Console.ReadLine();
        }
    }

}
