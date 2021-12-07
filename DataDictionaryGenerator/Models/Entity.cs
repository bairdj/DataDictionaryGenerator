namespace DataDictionaryGenerator.Models;

public class Entity
{
    public Entity(string name, string tableName)
    {
        Name = name;
        TableName = tableName;
        Fields = new List<Field>();
    }

    public string Name { get; set; }
    public string TableName { get; set; }
    public ICollection<Field> Fields { get; internal set; }
}