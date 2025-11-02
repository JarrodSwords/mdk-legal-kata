using MdkLegal.HelpDesk.Support.Services;

namespace MdkLegal.HelpDesk.Support.WebApi;

public class ConnectionStringProvider(IConfiguration configuration) : IConnectionStringProvider
{
    public string GetConnectionString() =>
        configuration
            .GetSection("ConnectionStrings")
            .Get<ConnectionStrings>().MdkLegal;
}
