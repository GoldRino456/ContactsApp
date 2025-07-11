using Microsoft.Extensions.Configuration;

namespace PhoneBook.GoldRino456;
public static class AppConfig
{
    public static bool FetchConnectionString(out string? connectionString)
    {
        string workingDirectory = Path.GetFullPath(@"..\..\..\");

        IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(workingDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

        connectionString = config.GetConnectionString("DefaultConnection");

        return !String.IsNullOrWhiteSpace(connectionString);
    }
}
