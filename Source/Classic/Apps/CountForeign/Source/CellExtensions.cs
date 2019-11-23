// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System.Drawing;

using DevExpress.Spreadsheet;

#endregion

// ReSharper disable UnusedMember.Global

namespace CountForeign
{
    public static class CellExtensions
    {
        public static Cell TextColor
            (
                this Cell cell,
                Color color
            )
        {
            cell.Font.Color = color;

            return cell;
        }

        public static Range TextColor
            (
                this Range range,
                Color color
            )
        {
            range.Font.Color = color;

            return range;
        }

        public static Row TextColor
            (
                this Row row,
                Color color
            )
        {
            row.Font.Color = color;

            return row;
        }

        public static Cell Center
            (
                this Cell cell
            )
        {
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

            return cell;
        }

        public static Cell Bold
            (
                this Cell cell
            )
        {
            cell.Font.Bold = true;

            return cell;
        }

        public static Range Bold
            (
                this Range range
            )
        {
            range.Font.Bold = true;

            return range;
        }

        public static Row Bold
            (
                this Row row
            )
        {
            row.Font.Bold = true;

            return row;
        }

        public static Cell RightJustify
            (
                this Cell cell
            )
        {
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;

            return cell;
        }

        public static Cell SetBorders
            (
                this Cell cell
            )
        {
            cell.Borders.SetAllBorders(Color.DarkBlue, BorderLineStyle.Thin);

            return cell;
        }

        public static Range SetBorders
            (
                this Range range
            )
        {
            range.Borders.SetAllBorders(Color.DarkBlue, BorderLineStyle.Thin);

            return range;
        }

        public static Cell Background
            (
                this Cell cell,
                Color color
            )
        {
            cell.FillColor = color;

            return cell;
        }

        public static Range Background
            (
                this Range range,
                Color color
            )
        {
            range.FillColor = color;

            return range;
        }

        public static Row Background
            (
                this Row row,
                Color color
            )
        {
            row.FillColor = color;

            return row;
        }

        public static Range Conditional2Colors
            (
                this Range range,
                Color minColor,
                Color maxColor
            )
        {
            var conditionalFormattings = range.Worksheet.ConditionalFormattings;
            var minPoint = conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax);
            var maxPoint = conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax);
            conditionalFormattings.AddColorScale2ConditionalFormatting
                (
                    range,
                    minPoint,
                    minColor,
                    maxPoint,
                    maxColor
                );

            return range;
        }
    }
}
