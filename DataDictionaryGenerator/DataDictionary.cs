using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DataDictionaryGenerator.Annotations;
using DataDictionaryGenerator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataDictionaryGenerator;

public class DataDictionary
{
    private DataDictionary(ICollection<Entity> entities, ICollection<Field> fields)
    {
        Entities = entities;
        Fields = fields;
    }

    public ICollection<Entity> Entities { get; }
    public ICollection<Field> Fields { get; }
    /// <summary>
    /// Creates a new DataDictionary object from the provided DbContext.
    /// You should <see href="https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli">configure the database provider</see> prior to using this method.
    /// 
    /// </summary>
    /// <param name="context">DBContext to generate data dictionary from</param>
    /// <returns></returns>
    public static DataDictionary FromDbContext(DbContext context)
    {
        List<Entity> entities = new();
        List<Field> fields = new();
        foreach (var tableEntity in GetIncludedEntityTypes(context.Model))
        {
            Entity entity = new(tableEntity.DisplayName(), tableEntity.GetTableName()!);
            foreach (var property in tableEntity.GetProperties())
            {
                Field field = new(entity, property.Name, property.GetColumnType(), property.ClrType,
                    new List<Constraint>());
                var displayName = FindDisplayAttribute(property);
                var underlyingClrType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                field.DisplayName = displayName?.Name;
                field.Description = displayName?.Description;
                field.Options = underlyingClrType.IsEnum ? GetFieldOptions(underlyingClrType) : null;
                field.Constraints = GetConstraints(property).ToList();
                field.Since = FindAttribute<SinceAttribute>(property)?.Date;
                field.Until = FindAttribute<UntilAttribute>(property)?.Date;
                if (property.IsForeignKey())
                {
                    // Only include non-composite keys
                    var pk = property.GetContainingForeignKeys().First().PrincipalKey;
                    if (pk.Properties.Count == 1)
                        field.RelatesTo = $"{pk.DeclaringEntityType.DisplayName()}.{pk.Properties[0].Name}";
                }

                field.Nullable = property.IsNullable;
                field.Unique = property.IsUniqueIndex();
                field.Default = property.GetDefaultValueSql();
                fields.Add(field);
                entity.Fields.Add(field);
            }

            entities.Add(entity);
        }

        return new DataDictionary(entities, fields);
    }

    private static IEnumerable<IEntityType> GetIncludedEntityTypes(IModel model)
    {
        return model.GetEntityTypes().Where(t => !t.IsAbstract() && t.GetTableMappings().Any());
    }

    public static Dictionary<string, string> GetFieldOptions(IReflect enumType)
    {
        var options = new Dictionary<string, string>();
        foreach (var enumProperty in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var key = enumProperty.GetRawConstantValue()?.ToString();
            if (key != null)
            {
                // Use display attribute for property, with fallback to underlying name
                var displayName = enumProperty.GetCustomAttribute<DisplayAttribute>()?.Name;
                options.Add(key, displayName ?? enumProperty.Name);
            }
        }

        return options;
    }

    private static DisplayAttribute? FindDisplayAttribute(IReadOnlyPropertyBase property)
    {
        return property.PropertyInfo?.GetCustomAttribute<DisplayAttribute>();
    }

    private static TAttribute? FindAttribute<TAttribute>(IReadOnlyPropertyBase property) where TAttribute : Attribute
    {
        return property.PropertyInfo?.GetCustomAttribute<TAttribute>();
    }

    private static IEnumerable<Constraint> GetConstraints(IReadOnlyPropertyBase property)
    {
        // Get System.Component.DataAnnotation validation
        var validationAttributes = property.PropertyInfo
            ?.GetCustomAttributes<ValidationAttribute>();
        if (validationAttributes == null) yield break;
        foreach (var validationAttribute in validationAttributes)
        {
            var constraint = GetConstraint(validationAttribute);
            if (constraint != null) yield return constraint;
        }
    }

    private static Constraint? GetConstraint(object attribute)
    {
        return attribute switch
        {
            MaxLengthAttribute maxLengthAttribute => new Constraint("Maximum length",
                maxLengthAttribute.Length.ToString()),
            MinLengthAttribute minLengthAttribute => new Constraint("Minimum length",
                minLengthAttribute.Length.ToString()),
            RegularExpressionAttribute regularExpressionAttribute => new Constraint("Regular expression",
                regularExpressionAttribute.Pattern),
            RangeAttribute rangeAttribute => new Constraint("Range",
                $"{rangeAttribute.Minimum} to {rangeAttribute.Maximum}"),
            UrlAttribute => new Constraint("URL", string.Empty),
            StringLengthAttribute stringLengthAttribute => new Constraint("Length",
                $"{stringLengthAttribute.MinimumLength} to {stringLengthAttribute.MaximumLength}"),
            PhoneAttribute => new Constraint("Phone number", string.Empty),
            EmailAddressAttribute => new Constraint("Email address", string.Empty),
            CreditCardAttribute => new Constraint("Credit card number", string.Empty),
            CompareAttribute compareAttribute => new Constraint("Equal to", compareAttribute.OtherProperty),
            _ => null
        };
    }
}