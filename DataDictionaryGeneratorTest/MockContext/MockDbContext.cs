using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataDictionaryGeneratorTest.MockContext;

internal class MockDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>();
        modelBuilder.Entity<CurrentAccount>();
        modelBuilder.Entity<SavingsAccount>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        optionsBuilder.UseSqlite(connection);
    }
}