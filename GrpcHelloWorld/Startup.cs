using System.Dynamic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Greet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcHelloWorld
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddTransient<GreeterService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/", async ctx =>
                {
                    var greeter = ctx.RequestServices.GetRequiredService<GreeterService>();

                    var stream = ctx.Request.BodyReader.AsStream();
                    var @string = await new StreamReader(stream).ReadToEndAsync();

                    var request = JsonSerializer.Parse<HelloRequest>(@string);

                    var body = JsonSerializer.ToString(await greeter.SayHello(request, null));

                    ctx.Response.ContentType = "application/json";
                    await ctx.Response.WriteAsync(body);
                });
                endpoints.MapGrpcService<GreeterService>();
            });
        }
    }
}
