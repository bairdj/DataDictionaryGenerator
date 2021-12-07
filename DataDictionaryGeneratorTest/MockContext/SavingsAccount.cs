using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace DataDictionaryGeneratorTest.MockContext;

internal class SavingsAccount : Account
{
    [Display(Name = "Interest rate", Description = "The current interest rate on the account.")]
    [Range(0, 10)]
    public decimal InterestRate { get; set; }
}