# Data Dictionary Generator

This library creates a rich data dictionary from a provided Entity Framework Core DbContext.

The intended use case is to provide users with a thorough description of a database without needing to read code.
The generated data dictionary includes information from System.ComponentModel.DataAnnotations annotations that provides greater detail than just inspecting the SQL schema.

Generated fields include:
- Entity name
- Table name (for table per hierarchy)
- Field display name
- Field description
- Mapped SQL column type
- CLR type
- Constraints (validation attributes)
- Collected from/until dates (using additional attributes)
- Nullability
- Foreign keys (non-composite)
- Options (when using enum fields)
- Uniqueness
- Default SQL values

The library contains built-in outputs to:
- CSV
- Word (docx)

The underlying DataDictionary object can be used to output the data dictionary in customised ways, such as an ASP.NET page or as part of an API.

## Usage
The library can be installed from [NuGet](https://www.nuget.org/packages/DataDictionaryGenerator/).

A minimal program looks like:
```
using DataDictionaryGenerator;
using DataDictionaryGenerator.Outputs;

var dbContext = new YourDbContext();
// Ensure that the DbContext is configured with a provider, by explicitly passing options or by overriding OnConfiguring within your DbContext class
// You could also resolve this from DI if applicable

var dictionary = DataDictionary.FromDbContext(dbContext);

// Output as CSV
await using var csvFile = File.Open("Data dictionary.csv", FileMode.Create);
await CsvOutput.GenerateAsync(dictionary, new StreamWriter(csvFile));

// Output Word
var wordDocument = WordOutput.Generate(dictionary);
wordDocument.Write(File.Open("Data dictionary.docx", FileMode.Create));
```
