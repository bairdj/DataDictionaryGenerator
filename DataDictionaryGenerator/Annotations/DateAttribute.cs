namespace DataDictionaryGenerator.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class DateAttribute : Attribute
{
    public DateOnly Date { get; }

    public DateAttribute(int year, int month, int day)
    {
        Date = new DateOnly(year, month, day);
    }
}