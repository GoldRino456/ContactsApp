﻿using PhoneBook.GoldRino456;
using PhoneBook.GoldRino456.Data;
using System.Text.RegularExpressions;
using Utilities.GoldRino456;

class Program
{
    static void Main()
    {
        if(!AppConfig.FetchConnectionString(out var connectionString))
        {
            throw new Exception("Could not find connection string.");
        }

        using var context = new ContactContext(connectionString);

        //TODO: Add Database Connection Check Here

        bool isAppRunning = true;
        while(isAppRunning)
        {
            var choice = DisplayMainMenu();

            switch(choice)
            {
                case MenuOptions.CreateContact:
                    MenuManager.ProcessCreateContact();
                    break;

                case MenuOptions.UpdateContact:
                    break;

                case MenuOptions.ViewContacts:
                    break;

                case MenuOptions.DeleteContact:
                    break;

                case MenuOptions.CreateCategory:
                    break;

                case MenuOptions.UpdateCategory:
                    break;

                case MenuOptions.DeleteCategory:
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
        foreach(var option in Enum.GetValues(typeof(MenuOptions)))
        {
            var displayText = Regex.Replace(option.ToString(), "(\\B[A-Z])", " $1"); //https://stackoverflow.com/questions/5796383/insert-spaces-between-words-on-a-camel-cased-token
            menuOptionPairs.Add(displayText, (int)option);
        }

        return (MenuOptions) DisplayUtils.PromptUserForIndexSelection("What Would You Like To Do?", menuOptionPairs);
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