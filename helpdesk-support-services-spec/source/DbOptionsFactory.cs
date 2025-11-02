using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MdkLegal.HelpDesk.Support.Services.Spec;

public static class DbOptionsFactory
{
    static DbOptionsFactory()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = Configuration["ConnectionStrings:MdkLegal"];

        DbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlServer(connectionString)
            .Options;
    }

    public static IConfiguration Configuration { get; }
    public static DbContextOptions<Context> DbContextOptions { get; }
}
