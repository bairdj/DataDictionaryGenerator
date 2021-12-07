namespace DataDictionaryGenerator.Models;

public class Field
{
    public Field(Entity entity, string sourceName, string sqlType, Type clrType, ICollection<Constraint> constraints)
    {
        Entity = entity;
        SourceName = sourceName;
        SqlType = sqlType;
        ClrType = clrType;
        Constraints = constraints;
    }

    public Entity Entity { get; set; }
    public string? DisplayName { get; set; }
    public string SourceName { get; set; }
    public string? Description { get; set; }
    public string SqlType { get; set; }
    public Type ClrType { get; set; }
    public ICollection<Constraint> Constraints { get; set; }
    public DateOnly? Since { get; set; }
    public DateOnly? Until { get; set; }
    public bool Nullable { get; set; }
    public string? RelatesTo { get; set; }
    public Dictionary<string, string>? Options { get; set; }
    public bool Unique { get; set; }
    public dynamic? Default { get; set; }

    public override string ToString()
    {
        return $"{SourceName} Type: {SqlType}";
    }
}