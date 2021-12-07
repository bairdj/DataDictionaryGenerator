namespace DataDictionaryGenerator.Annotations;

public class SinceAttribute : DateAttribute
{
    public SinceAttribute(int year, int month, int day) : base(year, month, day)
    {
    }
}