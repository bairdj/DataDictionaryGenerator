using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataDictionaryGenerator.Annotations;

namespace DataDictionaryGeneratorTest.MockContext;

internal class Customer
{
    public int CustomerId { get; set; }
    [MinLength(2)]
    [MaxLength(64)]
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    [Since(2021,1,1)]
    public string Address { get; set; } = null!;
    public ICollection<Account> Accounts { get; set; } = null!;
}