namespace DataDictionaryGeneratorTest.MockContext;

internal abstract class Account
{
    public int AccountId { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal Balance { get; set; }
    public bool Open { get; set; }
}