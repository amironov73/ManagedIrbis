using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

using ManagedIrbis;

static class Program
{
  private static DbManager db;
  private static IrbisConnection irbis;
  private static readonly Dictionary<string, List<string>> foundBindings 
    = new Dictionary<string, List<string>>(); 

  private static void ProcessCard (string cardNumber)
  {
    Console.Write($"{cardNumber}: ");
    var records = irbis.SearchRead($"\"NS={cardNumber}\"");
    var disciplines = db.SetCommand
      (
        @"select d.[name] from [disciplines] d
          inner join [book_bindings] b on b.[discipline] = d.[id]
        where b.[card] = @cardNumber",
        db.Parameter("cardNumber", cardNumber)
      )
      .ExecuteScalarList(new List<string>(), typeof(string));
    foreach (var record in records)
    {
      var field951 = record.Fields.GetFirstField(951);
      if (field951 != null)
      {
        var link = field951.GetFirstSubFieldValue('i');
        if (!string.IsNullOrEmpty(link))
        {
          var subtitile = record.FM(200, 'e');
          if (!string.IsNullOrEmpty(subtitile))
          {
            foreach (string discipline in disciplines)
            {
              if (!foundBindings.TryGetValue(discipline, out var books))
              {
                books = new List<string>();
                foundBindings.Add(discipline, books);
              }

              if (!books.Contains(subtitile))
              {
                Console.Write($"{subtitile}; ");
                books.Add(subtitile);
              }
            }
          }
        }
      }
    }

    Console.WriteLine();
  }
  
  public static void Main()
  {
    db = new DbManager
      (
        new SqlDataProvider(),
        "Server=(local);Database=booksupp;Trusted_Connection=True;"
      );
    irbis = new IrbisConnection("Server=localhost;user=librarian;password=secret;db=ISTU;");
    using (db)
    using (irbis)
    {
      var cards = db.SetCommand("select distinct [card] from [book_bindings]")
        .ExecuteScalarList(new List<string>(), typeof(string));
      Console.WriteLine($"Cards={cards.Count}");
      //var maxMfn = irbis.GetMaxMfn();
      //Console.WriteLine($"Max MFN={maxMfn}");

      foreach (string cardNumber in cards)
      {
        try
        {
          ProcessCard(cardNumber);
        }
        catch
        {
          // Do nothing
        }
      }
    }
    
    Console.WriteLine();
    Console.WriteLine("=========================");
    Console.WriteLine();
    Console.WriteLine();

    foreach (var discipline in foundBindings.Keys.OrderBy(_ => _))
    {
      Console.Write($"{discipline}\t");
      var books = foundBindings[discipline].OrderBy(_ => _);
      foreach (var book in books)
      {
        Console.Write($"{book}; ");
      }
      Console.WriteLine();
    }
    
  }
}
