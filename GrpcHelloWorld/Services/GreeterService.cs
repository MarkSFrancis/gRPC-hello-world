using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greet;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcHelloWorld
{
    public class GreeterService : Greeter.GreeterBase
    {
        public GreeterService(ILogger<GreeterService> logger)
        {
            Logger = logger;
        }

        public ILogger<GreeterService> Logger { get; }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
