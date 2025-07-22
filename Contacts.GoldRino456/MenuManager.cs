
using PhoneBook.GoldRino456.Data;
using Utilities.GoldRino456;

namespace PhoneBook.GoldRino456;

public static class MenuManager
{
    public static void ProcessCreateContact(ContactContext context)
    {
        ContactEntry contact = new();

        var name = DisplayUtils.PromptUserForStringInput("Please enter the contact's name: ");
        contact.Name = name;

        var phone = DisplayUtils.PromptUserForStringInput("Please enter the contact's phone number: ");
        contact.PhoneNumber = phone;

        var email = DisplayUtils.PromptUserForStringInput("Please enter the contact's email address: ");
        contact.Email = email;

        ContactCategory? category = null;
        var isRequestingCategoryPrompt = DisplayUtils.PromptUserForYesOrNoSelection("Would You Like To Add This Contact To A Category?");
   
        if(isRequestingCategoryPrompt)
        {
            category = PromptUserForCategorySelection(context);
        }

        contact.Category = category;

        ConfirmContactDetails(contact, context);
    }

    private static void ConfirmContactDetails(ContactEntry contact, ContactContext context)
    {
        var isCorrect = DisplayUtils.PromptUserForYesOrNoSelection("Are the details for this contact correct?");

        if(isCorrect)
        {
            context.Contacts.Add(contact);
            context.SaveChanges();
            return;
        }

        ProcessCreateContact(context);
    }

    public static ContactCategory? PromptUserForCategorySelection(ContactContext context)
    {
        List<ContactCategory> categories = context.ContactCategories.ToList();
        List<string> displayCategories = new();
        string emptyCategoryChoice = "Uncategorized (No Category)";


        foreach (ContactCategory category in categories)
        {
            displayCategories.Add(category.Name);
        }

        displayCategories.Add(emptyCategoryChoice);

        var categoryChoice = DisplayUtils.PromptUserForSelectionFromList("Please select a category: ", displayCategories);

        if(categoryChoice.Equals(emptyCategoryChoice))
        {
            return null;
        }

        return context.ContactCategories.Where(c => c.Name.Equals(categoryChoice)).First();
    }
}
