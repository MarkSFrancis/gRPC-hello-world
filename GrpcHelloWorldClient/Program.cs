using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Greet;
using Grpc.Core;

namespace GrpcHelloWorldClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var restClient = new HttpClient();

            var channel = new Channel("localhost:5001", ChannelCredentials.Insecure);
            var client = new Greeter.GreeterClient(channel);

            var request = new HelloRequest
            {
                Name = "Test Client"
            };

            var jsonRequest = JsonSerializer.ToString(request);

            var restRequest = await restClient.PostAsync(
                $"https://localhost:5002",
                new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            var restReplyRaw = await restRequest.Content.ReadAsStringAsync();

            var restReplyObj = JsonSerializer.Parse<HelloReply>(restReplyRaw);

            Console.WriteLine("Greeting from REST: " + restReplyObj.Message);

            var grpcReply = await client.SayHelloAsync(request);
            Console.WriteLine("Greeting: " + grpcReply.Message);

            await channel.ShutdownAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
