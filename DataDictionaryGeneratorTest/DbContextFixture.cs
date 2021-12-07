using System;
using DataDictionaryGeneratorTest.MockContext;
using Microsoft.EntityFrameworkCore;

namespace DataDictionaryGeneratorTest;

public class DbContextFixture : IDisposable
{
    public DbContext Context { get; }

    public DbContextFixture()
    {
        Context = new MockDbContext();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}