using System.Globalization;
using CsvHelper;
using JetBrains.Annotations;

namespace DataDictionaryGenerator.Outputs;

public static class CsvOutput
{
    [PublicAPI]
    public static async Task GenerateAsync(DataDictionary dictionary, TextWriter textWriter)
    {
        await using var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
        await writer.WriteRecordsAsync(BuildCsvRows(dictionary));
    }

    private static IEnumerable<CsvRow> BuildCsvRows(DataDictionary dictionary)
    {
        return dictionary.Fields.Select(field =>
            new CsvRow(field.Entity.Name, field.Entity.TableName, field.DisplayName ?? field.SourceName,
                field.SourceName, field.SqlType, field.ClrType, field.Nullable, field.Unique)
            {
                Description = field.Description,
                Constraints = string.Join(", ", field.Constraints.Select(c => $"{c.Name}: {c.Description}")),
                Since = field.Since,
                Until = field.Until,
                RelatesTo = field.RelatesTo,
                Options = field.Options != null
                    ? string.Join(", ", field.Options!.Select(kvp => $"{kvp.Key} = {kvp.Value}"))
                    : null,
                Default = field.Default
            });
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    private class CsvRow
    {
        public CsvRow(string entity, string table, string displayName, string sourceName, string sqlType, Type clrType,
            bool nullable, bool unique)
        {
            Entity = entity;
            Table = table;
            DisplayName = displayName;
            SourceName = sourceName;
            SqlType = sqlType;
            ClrType = System.Nullable.GetUnderlyingType(clrType)?.Name ?? clrType.Name;
            Nullable = nullable;
            Unique = unique;
        }

        public string Entity { get; }
        public string Table { get; }
        public string DisplayName { get; }
        public string SourceName { get; }
        public string? Description { get; set; }
        public string SqlType { get; }
        public string ClrType { get; }
        public string? Constraints { get; set; }
        public DateOnly? Since { get; set; }
        public DateOnly? Until { get; set; }
        public bool Nullable { get; }
        public string? RelatesTo { get; set; }
        public string? Options { get; set; }
        public bool Unique { get; }
        public dynamic? Default { get; set; }
    }
}