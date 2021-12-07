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