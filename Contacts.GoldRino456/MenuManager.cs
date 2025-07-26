
using PhoneBook.GoldRino456.Data;
using System.Numerics;
using Utilities.GoldRino456;

namespace PhoneBook.GoldRino456;

public static class MenuManager
{
    public static void ProcessCreateContact(ContactContext context)
    {
        ContactEntry contact = new();
        DisplayUtils.ClearScreen();
        CollectContactDetails(contact, context, true);
    }

    private static void CollectContactDetails(ContactEntry contact, ContactContext context, bool isNewContact)
    {
        while(true)
        {
            bool isCancellingOperation = ProcessNameInput(contact);
            if (isCancellingOperation) { return; }

            isCancellingOperation = ProcessPhoneInput(contact);
            if (isCancellingOperation) { return; }

            isCancellingOperation = ProcessEmailInput(contact);
            if (isCancellingOperation) { return; }

            ContactCategory? category = null;
            var isRequestingCategoryPrompt = DisplayUtils.PromptUserForYesOrNoSelection("Would You Like To Add This Contact To A Category?");

            if (isRequestingCategoryPrompt)
            {
                category = PromptUserForCategorySelection(context, "Uncategorized (No Category)");
            }

            contact.Category = category;

            if (ConfirmContactDetails(contact, context, isNewContact))
            {
                return;
            }
        }
    }

    public static void ProcessUpdateContact(ContactContext context)
    {
        DisplayUtils.ClearScreen();
        var isCancellingOperation = PromptUserForContactSelection(context, out ContactEntry? selectedContact);

        if(isCancellingOperation)
        { 
            return; 
        }

        DisplaySingleContact(selectedContact);
        CollectContactDetails(selectedContact, context, false);
    }

    private static bool PromptUserForContactSelection(ContactContext context, out ContactEntry? selectedContact)
    {
        var contacts = GetContactsByCategory(context);

        if (contacts.Count <= 0)
        {
            selectedContact = null;
            DisplayUtils.DisplayMessageToUser("There are not any contacts to display.");
            DisplayUtils.PressAnyKeyToContinue();
            return true;
        }

        Dictionary<string, ContactEntry?> contactPairs = new();
        foreach (var contact in contacts)
        {
            contactPairs.Add(contact.Name, contact);
        }
        contactPairs.Add("Go Back To Menu", null);

        var selection = DisplayUtils.PromptUserForSelectionFromList("Which contact would you like to update?", contactPairs.Keys.ToList());

        if(selection.Equals("Go Back To Menu"))
        {
            selectedContact = null;
            return true;
        }
        selectedContact = contactPairs[selection];
        return false;
    }

    public static void ProcessViewContacts(ContactContext context)
    {
        DisplayUtils.ClearScreen();
        var contacts = GetContactsByCategory(context);

        string[] columns = ["Name", "Email", "Phone Number", "Category"];
        List<string[]> rows = new();

        foreach (var contact in contacts)
        {
            var categoryString = contact.Category != null ? contact.Category.Name : "N/A";
            string[] row = [contact.Name, contact.Email, contact.PhoneNumber, categoryString];
            rows.Add(row);
        }

        DisplayUtils.DisplayListAsTable(columns, rows);
        DisplayUtils.PressAnyKeyToContinue();
    }

    public static void ProcessCreateCategory(ContactContext context)
    {
        while(true)
        {
            DisplayUtils.ClearScreen();

            ContactCategory category = new();
            bool isCancellingOperation = false;

            isCancellingOperation = ProcessCategoryInput(category);

            if (isCancellingOperation)
            {
                return;
            }

            if(ConfirmCategoryDetails(category, context, true))
            {
                return;
            }
        }
    }

    public static void ProcessUpdateCategory(ContactContext context)
    {
        while(true)
        {
            DisplayUtils.ClearScreen();

            if (!CheckForExistingCategories(context))
            {
                DisplayUtils.DisplayMessageToUser("No categories exist to update!");
                DisplayUtils.PressAnyKeyToContinue();
                return;
            }

            var category = PromptUserForCategorySelection(context, "Go Back To Menu");
            if (category == null)
            {
                return;
            }

            DisplaySingleCategory(category);
            var isCancellingOperation = ProcessCategoryInput(category);

            if (isCancellingOperation)
            {
                return;
            }

            if (ConfirmCategoryDetails(category, context, false))
            {
                return;
            }
        }
    }

    public static void ProcessDeleteContact(ContactContext context)
    {
        DisplayUtils.ClearScreen();
        var isCancellingOperation = PromptUserForContactSelection(context, out ContactEntry? selectedContact);

        if (isCancellingOperation)
        {
            return;
        }

        DisplayUtils.ClearScreen();
        DisplaySingleContact(selectedContact);
        var isDeleteConfirmed = DisplayUtils.PromptUserForYesOrNoSelection("Are you sure you want to delete this contact?");

        if (isDeleteConfirmed)
        {
            context.Contacts.Remove(selectedContact);
            context.SaveChanges();
        }
    }

    private static bool ProcessEmailInput(ContactEntry contact)
    {
        string email;

        while(true)
        {
            email = DisplayUtils.PromptUserForStringInput("Please enter the contact's email address (or enter '0' to return to the menu): ");

            if (email.Equals("0"))
            {
                return true;
            }

            if (ValidationUtils.ValidateEmailAddress(email))
            {
                break;
            }
            else
            {
                DisplayUtils.DisplayMessageToUser("Email is invalid. Please ensure email address is in the following format: johndoe@myemail.com");
            }
        }

        contact.Email = email;
        return false;
    }

    private static bool ProcessPhoneInput(ContactEntry contact)
    {
        string phone;

        while(true)
        {
            phone = DisplayUtils.PromptUserForStringInput("Please enter the contact's phone number (or enter '0' to return to the menu): ");

            if (phone.Equals("0")) //Return to menu
            {
                return true;
            }

            if(ValidationUtils.ValidatePhoneNumber(phone))
            {
                break;
            }
            else
            {
                DisplayUtils.DisplayMessageToUser("Phone Number is invalid. Please ensure phone number is in the following format: (123)-456-7890, 123-456-7890, or 1234567890.");
            }
        }

        contact.PhoneNumber = phone;
        return false;
    }

    private static bool ProcessNameInput(ContactEntry contact)
    {
        var name = DisplayUtils.PromptUserForStringInput("Please enter the contact's name (or enter '0' to return to the menu): ");

        if (name.Equals("0"))
        {
            return true;
        }

        contact.Name = name;
        return false;
    }

    private static bool ProcessCategoryInput(ContactCategory category)
    {
        string categoryName = DisplayUtils.PromptUserForStringInput("Please enter the category's name (or enter '0' to return to the menu): ");

        if(categoryName.Equals("0"))
        {
            return true;
        }

        category.Name = categoryName;
        return false;
    }

    private static bool ConfirmContactDetails(ContactEntry contact, ContactContext context, bool isNewContact)
    {
        DisplayUtils.ClearScreen();
        DisplayUtils.DisplayMessageToUser($"Contact Name: {contact.Name} \nEmail: {contact.Email} \nPhone Number: {contact.PhoneNumber}");

        if(contact.Category != null)
        {
            DisplayUtils.DisplayMessageToUser($"Category: {contact.Category}");
        }

        var isCorrect = DisplayUtils.PromptUserForYesOrNoSelection("Are the details for this contact correct?");

        if(isCorrect)
        {
            if(isNewContact)
            {
                context.Contacts.Add(contact);
                context.SaveChanges();
                return true;
            }
            else
            {
                context.Contacts.Update(contact);
                context.SaveChanges();
                return true;
            }
        }

        return false;
    }
    
    private static bool ConfirmCategoryDetails(ContactCategory category, ContactContext context, bool isNewCategory)
    {
        DisplayUtils.ClearScreen();
        DisplayUtils.DisplayMessageToUser($"New Category Name: {category.Name}");

        var isCorrect = DisplayUtils.PromptUserForYesOrNoSelection("Is the category name above correct?");

        if (isCorrect)
        {
            if(isNewCategory)
            {
                context.ContactCategories.Add(category);
                context.SaveChanges();
                return true;
            }
            else
            {
                context.ContactCategories.Update(category);
                context.SaveChanges();
                return true;
            }
        }

        return false;
    }

    private static void DisplaySingleContact(ContactEntry contact)
    {
        DisplayUtils.ClearScreen();
        string[] columns = ["Name", "Email", "Phone Number", "Category"];
        List<string[]> rows = new();

        var categoryString = contact.Category != null ? nameof(contact.Category) : "N/A";
        string[] row = [contact.Name, contact.Email, contact.PhoneNumber, categoryString];
        rows.Add(row);

        DisplayUtils.DisplayListAsTable(columns, rows);
    }

    private static void DisplaySingleCategory(ContactCategory category)
    {
        DisplayUtils.ClearScreen();
        DisplayUtils.DisplayMessageToUser($"Category Name: {category.Name}");
    }

    private static List<ContactEntry> GetContactsByCategory(ContactContext context)
    {
        List<ContactEntry> contacts;
        var category = PromptUserForCategorySelection(context, "Uncategorized (No Category)");

        if (category != null)
        {
            contacts = context.Contacts.Where(c => c.Category == category).ToList();
        }
        else
        {
            contacts = context.Contacts.ToList();
        }

        return contacts;
    }

    private static ContactCategory? PromptUserForCategorySelection(ContactContext context, string emptyCategoryText)
    {
        List<ContactCategory> categories = context.ContactCategories.ToList();

        if(categories.Count <= 0)
        {
            return null;
        }

        List<string> displayCategories = new();


        foreach (ContactCategory category in categories)
        {
            displayCategories.Add(category.Name);
        }

        displayCategories.Add(emptyCategoryText);

        var categoryChoice = DisplayUtils.PromptUserForSelectionFromList("Please select a category: ", displayCategories);

        if(categoryChoice.Equals(emptyCategoryText))
        {
            return null;
        }

        return context.ContactCategories.Where(c => c.Name.Equals(categoryChoice)).First();
    }

    private static bool CheckForExistingCategories(ContactContext context)
    {
        return context.ContactCategories.Any();
    }

    private static bool CheckForExistingContacts(ContactContext context)
    {
        return context.Contacts.Any();
    }
}
