namespace DataDictionaryGenerator.Annotations;

public class UntilAttribute : DateAttribute
{
    /// <summary>
    /// Annotates field to show when it stopped being collected
    /// </summary>
    /// <param name="year">Year that field stopped being collected</param>
    /// <param name="month">Month that field stopped being collected</param>
    /// <param name="day">Day that field stopped being collected</param>
    public UntilAttribute(int year, int month, int day) : base(year, month, day)
    {
    }
}