using PhoneBook.GoldRino456;
using System.Text.RegularExpressions;
using Utilities.GoldRino456;

class Program
{
    static void Main()
    {
        if (!AppConfig.FetchConnectionString(out var connectionString))
        {
            throw new Exception("Could not find connection string.");
        }

        //TODO: Add Database Connection Check Here

        bool isAppRunning = true;
        while (isAppRunning)
        {
            DisplayUtils.ClearScreen();
            var choice = DisplayMainMenu();

            switch (choice)
            {
                case MenuOptions.CreateContact:
                    MenuManager.ProcessCreateContact(connectionString);
                    break;

                case MenuOptions.UpdateContact:
                    MenuManager.ProcessUpdateContact(connectionString);
                    break;

                case MenuOptions.ViewContacts:
                    MenuManager.ProcessViewContacts(connectionString);
                    break;

                case MenuOptions.DeleteContact:
                    MenuManager.ProcessDeleteContact(connectionString);
                    break;

                case MenuOptions.CreateCategory:
                    MenuManager.ProcessCreateCategory(connectionString);
                    break;

                case MenuOptions.UpdateCategory:
                    MenuManager.ProcessUpdateCategory(connectionString);
                    break;

                case MenuOptions.DeleteCategory:
                    MenuManager.ProcessDeleteCategory(connectionString);
                    break;

                case MenuOptions.Quit:
                    isAppRunning = false;
                    break;

            }
        }
    }

    private static MenuOptions DisplayMainMenu()
    {
        Dictionary<string, int> menuOptionPairs = new();
        foreach (var option in Enum.GetValues(typeof(MenuOptions)))
        {
            var displayText = Regex.Replace(option.ToString(), "(\\B[A-Z])", " $1"); //https://stackoverflow.com/questions/5796383/insert-spaces-between-words-on-a-camel-cased-token
            menuOptionPairs.Add(displayText, (int)option);
        }

        return (MenuOptions)DisplayUtils.PromptUserForIndexSelection("What Would You Like To Do?", menuOptionPairs);
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