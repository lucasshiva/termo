using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Termo.Api.Repositories;
using TUnit.AspNetCore;

namespace Termo.Api.Tests.Integration;

public class WebApplicationFactory : TestWebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Services from `Program.cs` are registered by default.
        // Here we can override/replace what we need.
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();
            services.AddSingleton<IWordRepository, FakeWordRepository>();
        });
    }
}

public abstract class ControllerTestsBase : WebApplicationTest<WebApplicationFactory, Program> { }
