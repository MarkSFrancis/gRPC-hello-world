using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Greet;
using Grpc.Core;

namespace GrpcHelloWorldClient
{
    public class Benchmark
    {
        public string RestEndpoint { get; set; }

        public string GrpcEndpoint { get; set; }

        public HttpClient RestClient { get; set; }

        public Channel GrpcClient { get; set; }

        public HelloRequest Request { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            RestClient = new HttpClient();
            RestEndpoint = "https://localhost:5002";
            GrpcEndpoint = "localhost:5001";
            GrpcClient = new Channel("localhost:5001", ChannelCredentials.Insecure);

            Request = new HelloRequest
            {
                Name = "Test Client"
            };
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            await GrpcClient.ShutdownAsync();
        }

        [Benchmark]
        public async Task<HelloReply> Rest()
        {
            var jsonRequest = JsonSerializer.ToBytes(Request);

            var content = new ByteArrayContent(jsonRequest);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await RestClient.PostAsync(RestEndpoint, content);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.ReadAsync<HelloReply>(responseBody);

            return result;
        }

        [Benchmark]
        public async Task<HelloReply> GrpcDynamicChannel()
        {
            var channel = new Channel("localhost:5001", ChannelCredentials.Insecure);
            var client = new Greeter.GreeterClient(channel);

            var grpcReply = await client.SayHelloAsync(Request);

            await channel.ShutdownAsync();

            return grpcReply;
        }

        [Benchmark]
        public async Task<HelloReply> GrpcSharedChannel()
        {
            var client = new Greeter.GreeterClient(GrpcClient);

            var grpcReply = await client.SayHelloAsync(Request);

            return grpcReply;
        }
    }
}