using JetBrains.Annotations;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;

namespace DataDictionaryGenerator.Outputs;

public static class WordOutput
{
    [PublicAPI]
    public static XWPFDocument Generate(DataDictionary dictionary, string fontFamily = "Arial",
        ST_PageOrientation orientation = ST_PageOrientation.landscape)
    {
        const ulong margin = 20;
        var document = new XWPFDocument
        {
            Document = new CT_Document
            {
                body = new CT_Body
                {
                    sectPr = new CT_SectPr
                    {
                        pgMar = new CT_PageMar
                        {
                            left = margin * 20, right = margin * 20, top = (margin * 20).ToString(), bottom =
                                (margin * 20).ToString()
                        },
                        pgSz = new CT_PageSz {orient = orientation, w = 842 * 20, h = 595 * 20}
                    }
                }
            }
        };
        var styles = document.CreateStyles();
        var fonts = new CT_Fonts
        {
            ascii = fontFamily
        };
        styles.SetDefaultFonts(fonts);
        var titleParagraph = document.CreateParagraph();
        var titleRun = titleParagraph.CreateRun();
        titleRun.SetText("Data Dictionary");
        titleRun.FontSize = 24.0;
        var x = 0;
        foreach (var entity in dictionary.Entities)
        {
            var entityTitle = document.CreateParagraph();
            entityTitle.SpacingAfterLines = 1;
            var entityTitleRun = entityTitle.CreateRun();
            entityTitleRun.FontSize = 16.0;
            entityTitleRun.SetText(entity.Name);
            var table = document.CreateTable(entity.Fields.Count + 1, 9);
            table.GetTrPr().tblW = new CT_TblWidth {w = "100%"};
            // Add table headers
            var headerRow = table.GetRow(0);
            headerRow.IsRepeatHeader = true;
            headerRow.GetCell(0).SetText("Name");
            headerRow.GetCell(1).SetText("Description");
            headerRow.GetCell(2).SetText("Type");
            headerRow.GetCell(3).SetText("SQL Column");
            headerRow.GetCell(4).SetText("SQL Type");
            headerRow.GetCell(5).SetText("Nullable");
            headerRow.GetCell(6).SetText("Constraints");
            headerRow.GetCell(7).SetText("Options");
            headerRow.GetCell(8).SetText("Relates to");
            var i = 1;
            foreach (var field in entity.Fields)
            {
                var row = table.GetRow(i);
                row.GetCell(0).SetText(field.DisplayName ?? field.SourceName);
                row.GetCell(1).SetText(field.Description);
                row.GetCell(2).SetText(Nullable.GetUnderlyingType(field.ClrType)?.Name ?? field.ClrType.Name);
                row.GetCell(3).SetText(field.SourceName);
                row.GetCell(4).SetText(field.SqlType);
                row.GetCell(5).SetText(field.Nullable ? "Yes" : "No");
                var constraintIndex = 0;
                foreach (var constraint in field.Constraints)
                {
                    var constraintParagraph = constraintIndex == 0
                        ? row.GetCell(6).GetParagraphArray(0)
                        : row.GetCell(6).AddParagraph();
                    var constraintLabelRun = constraintParagraph.CreateRun();
                    constraintLabelRun.IsBold = true;
                    constraintLabelRun.SetText($"{constraint.Name}: ");
                    var constraintDescriptionRun = constraintParagraph.CreateRun();
                    constraintDescriptionRun.SetText(constraint.Description);
                    constraintIndex++;
                }

                if (field.Options != null)
                {
                    var optionsCell = row.GetCell(7);
                    var o = 0;
                    foreach (var (key, value) in field.Options)
                    {
                        var optionsParagraph = o == 0 ? optionsCell.Paragraphs[0] : optionsCell.AddParagraph();
                        var optionsRun = optionsParagraph.CreateRun();
                        optionsRun.SetText($"{key} = {value}");
                        o++;
                    }
                }

                row.GetCell(8).SetText(field.RelatesTo);
                i++;
            }

            var breakParagraph = document.CreateParagraph();
            var breakRun = breakParagraph.CreateRun();
            x++;
            if (x != dictionary.Entities.Count) breakRun.AddBreak(BreakType.PAGE);
        }

        return document;
    }
}