// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Usings directives

using System;

using DevExpress.Spreadsheet;
using JetBrains.Annotations;

#endregion

namespace CoronaBooks
{
  public sealed class ExcelOutput
  {
    private static Workbook workbook;
    private static Worksheet worksheet;
    private static int currentLine;
    public ExcelOutput()
    {
      workbook = new Workbook
      {
        Unit = DevExpress.Office.DocumentUnit.Millimeter
      };
      worksheet = workbook.Worksheets[0];
      currentLine = 0;
    }

    public Cell WriteCell(int column, string value)
    {
      if (int.TryParse(value, out var number))
      {
        return WriteCell(column, number);
      }

      Cell cell = worksheet.Cells[currentLine, column];
      cell.Value = value;
      cell.NumberFormat = "@";
      return cell;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public Cell WriteCell(int column, int value)
    {
      Cell cell = worksheet.Cells[currentLine, column];
      cell.Value = value;
      return cell;
    }

    [NotNull]
    // ReSharper disable once UnusedMember.Global
    public Cell WriteCell(int column, double value)
    {
      Cell cell = worksheet.Cells[currentLine, column];
      cell.Value = value;
      cell.NumberFormat = "0.00";
      return cell;
    }

    [NotNull]
    public Cell WriteCell(int column, DateTime value)
    {
      Cell cell = worksheet.Cells[currentLine, column];
      cell.Value = value;
      cell.NumberFormat = "d mmm yyyy";
      return cell;  
    }
    
    [NotNull]
    public Cell WriteLine(string text)
    {
      Cell result = WriteCell(0, text);
      ++currentLine;
      return result;
    }

    public void SetColumnWidth(params int[] columns)
    {
      for (int i = 0; i < columns.Length; i++)
      {
        worksheet.Columns[i].Width = columns[i];
      }
    }

    public void MergeCells(int first, int last)
    {
      Range range = worksheet.Range.FromLTRB(first, currentLine, last, currentLine);
      worksheet.MergeCells(range);
    }

    public void WriteLine()
    {
      ++currentLine;
    }

    public void PageBreak()
    {
      worksheet.HorizontalPageBreaks.Add(currentLine);
    }
    public void Save(string filename)
    {
      workbook.SaveDocument(filename);
    }
  }
}
