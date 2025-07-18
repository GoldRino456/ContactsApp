
using Utilities.GoldRino456;

namespace PhoneBook.GoldRino456;

public static class MenuManager
{
    public static void ProcessCreateContact()
    {
        var name = DisplayUtils.PromptUserForStringInput("Please enter the contact's name: ");
        var phone = DisplayUtils.PromptUserForStringInput("Please enter the contact's phone number: ");
        var email = DisplayUtils.PromptUserForStringInput("Please enter the contact's email address: ");
        //Add option to place contact in category here.

        //Create contact here.
    }
}
