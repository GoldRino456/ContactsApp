using PhoneBook.GoldRino456;
using PhoneBook.GoldRino456.Data;

class Program
{
    static void Main()
    {
        if(AppConfig.FetchConnectionString(out var connectionString))
        {
            throw new Exception("Could not find connection string.");
        }

        using var context = new ContactContext(connectionString);

        //Display Menu Options
    }

    enum MenuOptions
    {
        CreateContact,
        UpdateContact,
        ViewContacts,
        DeleteContact,
        CreateCategory,
        UpdateCategory,
        DeleteCategory,
        Quit
    }
}