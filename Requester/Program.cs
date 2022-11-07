using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Requester
{
    internal class Program
    {
        private const string PORTAL_URL = "http://localhost:5000/Home/Index";
        private static readonly CancellationTokenSource source = new CancellationTokenSource();
        private static readonly CancellationToken token = source.Token;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (o, e) => source.Cancel();

            do
            {
                Console.Write("Número de Requests concorrentes: ");

                int requestQuantity;
                bool didParse = int.TryParse(Console.ReadLine(), out requestQuantity);

                if (!didParse)
                    requestQuantity = 50;
                    
                if (requestQuantity == 0)
                    requestQuantity = 1;

                Console.WriteLine($"Número de Requests assumidas: {requestQuantity}");

                var tasks = new Task[requestQuantity];

                for (int i = 0; i < requestQuantity; i++)
                {
                    var x = i;

                    tasks[i] = Task.Run(() => DoRequest(x));
                }

                Task.WaitAll(tasks);
                Console.WriteLine(Environment.NewLine);
            }
            while (!token.IsCancellationRequested);
        }

        private async static Task DoRequest(int requestId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.GetStringAsync(PORTAL_URL);
                    Console.WriteLine($"Request {requestId} => Ok: {!string.IsNullOrEmpty(result)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request {requestId} => Erro: {ex.Message}");
            }
        }
    }
}