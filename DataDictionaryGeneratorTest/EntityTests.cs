using DataDictionaryGenerator;
using Xunit;

namespace DataDictionaryGeneratorTest;

public class EntityTests : IClassFixture<DbContextFixture>
{
    private readonly DataDictionary _dataDictionary;
    public EntityTests(DbContextFixture contextFixture)
    {
        _dataDictionary = DataDictionary.FromDbContext(contextFixture.Context);
    }

    [Fact]
    public void TableCount_EqualsThree()
    {
        var count = _dataDictionary.Entities.Count;
        Assert.Equal(3, count);
    }

    [Theory]
    [InlineData("Customer")]
    [InlineData("CurrentAccount")]
    [InlineData("SavingsAccount")]
    public void Tables_ContainsCustomer(string tableName)
    {
        Assert.Contains(_dataDictionary.Entities, t => t.Name.Equals(tableName));
    }



}