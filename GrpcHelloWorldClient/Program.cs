using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using Greet;
using Grpc.Core;

namespace GrpcHelloWorldClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
