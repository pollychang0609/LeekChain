using System;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LeekChain
{
    public class P2PServer: WebSocketBehavior
    {
        private WebSocketServer _wss;


        public void Start()
        {
            var address = $"ws://127.0.0.1:{Program.Port}";
            _wss = new WebSocketServer(address);
            _wss.AddWebSocketService<P2PServer>("/Blockchain");
            _wss.Start();

            Console.WriteLine($"Server started: {address}");
            
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                var newChain = JsonSerializer.Deserialize<Blockchain>(e.Data);
                if(newChain is not null
                    && newChain.IsValid()
                    && newChain.Chain.Count > Program.CurrentChain.Chain.Count)
                {
                    Console.WriteLine($"{nameof(P2PServer)}::Sync new chain");
                    var newTransactions = new List<Transaction>();
                    Program.CurrentChain = newChain;
                }
            }
        }
    }
}
