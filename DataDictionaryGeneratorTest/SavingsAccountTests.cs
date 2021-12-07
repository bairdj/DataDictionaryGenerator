using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDictionaryGenerator;
using DataDictionaryGenerator.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DataDictionaryGeneratorTest;

public class SavingsAccountTests : IClassFixture<DbContextFixture>
{
    private readonly Entity _entity;
    public SavingsAccountTests(DbContextFixture fixture)
    {
        var dataDictionary = DataDictionary.FromDbContext(fixture.Context);
        var table = dataDictionary.Entities.FirstOrDefault(t => t.Name.Equals("SavingsAccount"));

        _entity = table ?? throw new Exception("SavingsAccount table not found");
    }

    [Fact]
    public void InterestRate_DisplayName()
    {
        Assert.Equal("Interest rate", _entity.Fields.FirstOrDefault(f => f.SourceName.Equals("InterestRate"))?.DisplayName);
    }


}