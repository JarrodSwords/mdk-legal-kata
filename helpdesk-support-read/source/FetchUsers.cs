using MdkLegal.HelpDesk.Support.Services;
using Microsoft.Data.SqlClient;

namespace MdkLegal.HelpDesk.Support.Read;

public record FetchUsers : Query;

public class FetchUsersHandler(IConnectionStringProvider provider)
    : IQueryHandler<FetchUsers, Result<IEnumerable<User>>>
{
    private const string Query =
        """
        SELECT Id
             , Email
             , Name
          FROM [User]
        """;

    public Result<IEnumerable<User>> Handle(FetchUsers query)
    {
        using var connection = new SqlConnection(provider.GetConnectionString());

        try
        {
            connection.Open();

            return connection.Query<User>(Query, query).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
}
