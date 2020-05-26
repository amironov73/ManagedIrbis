// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System.Drawing;

using DevExpress.Spreadsheet;

using JetBrains.Annotations;

#endregion

// ReSharper disable UnusedMember.Global

namespace CoronaBooks
{
    public static class CellExtensions
    {
        [NotNull]
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static Cell TextColor
            (
                [NotNull] this Cell cell,
                Color color
            )
        {
            cell.Font.Color = color;

            return cell;
        }

        [NotNull]
        public static Range TextColor
            (
                [NotNull] this Range range,
                Color color
            )
        {
            range.Font.Color = color;

            return range;
        }

        [NotNull]
        public static Row TextColor
            (
                [NotNull] this Row row,
                Color color
            )
        {
            row.Font.Color = color;

            return row;
        }

        [NotNull]
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static Cell Center
            (
                [NotNull] this Cell cell
            )
        {
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

            return cell;
        }

        [NotNull]
        public static Cell Bold
            (
                [NotNull] this Cell cell
            )
        {
            cell.Font.Bold = true;

            return cell;
        }

        [NotNull]
        public static Range Bold
            (
                [NotNull] this Range range
            )
        {
            range.Font.Bold = true;

            return range;
        }

        [NotNull]
        public static Row Bold
            (
                [NotNull] this Row row
            )
        {
            row.Font.Bold = true;

            return row;
        }

        [NotNull]
        public static Cell RightJustify
            (
                [NotNull] this Cell cell
            )
        {
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;

            return cell;
        }

        [NotNull]
        public static Cell SetBorders
            (
                [NotNull] this Cell cell
            )
        {
            cell.Borders.SetAllBorders(Color.DarkBlue, BorderLineStyle.Thin);

            return cell;
        }

        [NotNull]
        public static Range SetBorders
            (
                [NotNull] this Range range
            )
        {
            range.Borders.SetAllBorders(Color.DarkBlue, BorderLineStyle.Thin);

            return range;
        }

        [NotNull]
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static Cell Background
            (
                [NotNull] this Cell cell,
                Color color
            )
        {
            cell.FillColor = color;

            return cell;
        }

        [NotNull]
        public static Range Background
            (
                [NotNull] this Range range,
                Color color
            )
        {
            range.FillColor = color;

            return range;
        }

        [NotNull]
        public static Row Background
            (
                [NotNull] this Row row,
                Color color
            )
        {
            row.FillColor = color;

            return row;
        }

        [NotNull]
        public static Cell FontSize
            (
                [NotNull] this Cell cell,
                double size
            )
        {
            cell.Font.Size = size;
            return cell;
        }

        [NotNull]
        public static Row Row
            (
                [NotNull] this Cell cell
            )
        {
            return cell.Worksheet.Rows[cell.RowIndex];
        }

        [NotNull]
        public static Cell WrapText
            (
                [NotNull] this Cell cell
            )
        {
            cell.Alignment.WrapText = true;
            return cell;
        }

        [NotNull]
        public static Range Conditional2Colors
            (
                [NotNull] this Range range,
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
