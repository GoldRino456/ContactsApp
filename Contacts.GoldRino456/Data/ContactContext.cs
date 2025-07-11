using Microsoft.EntityFrameworkCore;

namespace PhoneBook.GoldRino456.Data;
public class ContactContext : DbContext
{
    public DbSet<ContactEntry> Contacts { get; set; }
    public DbSet<ContactCategory> ContactCategories { get; set; }

    public string DbPath { get; }

    public ContactContext(string connectionString)
    {
        DbPath = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(DbPath);
}
