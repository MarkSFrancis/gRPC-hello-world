# gRPC-hello-world
A sample app of gRPC and REST working side-by-side on an aspnetcore server

Support for both scenarios requires different HTTP versions and security levels. See the [Program.cs](https://github.com/MarkSFrancis/gRPC-hello-world/blob/master/GrpcHelloWorld/Program.cs#L26) for how this is set up in Kestrel

# Benchmark Results

To review gRPC, I benchmarked it against traditional REST with JSON (using HTTP/2).

The dynamic channel recreates its connection to the server for each run, whereas the shared channel shares one constant channel across all requests

``` ini
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.706 (1803/April2018Update/Redstone4)

Frequency=2062499 Hz, Resolution=484.8487 ns, Timer=TSC
.NET Core SDK=3.0.100-preview5-011568
```

|             Method |           Mean |     Error |   StdDev |         Median |
|------------------- |---------------:|----------:|---------:|---------------:|
|               Rest |       696.2 us | 110.76 us | 314.2 us |       572.6 us |
| GrpcDynamicChannel | 1,002,969.5 us | 650.00 us | 608.0 us | 1,002,891.6 us |
|  GrpcSharedChannel |       432.2 us |  52.89 us | 152.6 us |       376.0 us |
