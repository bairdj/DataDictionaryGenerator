namespace DataDictionaryGenerator.Models;

public class Constraint
{
    public Constraint(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}