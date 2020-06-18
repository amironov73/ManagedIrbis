// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using AM;
using AM.Configuration;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using ManagedIrbis;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CoronaBooks
{
  /// <summary>
  /// Студенты по данным Отдела кадров.
  /// </summary>
  [TableName("students")]
  [UsedImplicitly]
  public class Student 
  {
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [PrimaryKey]
    public int Id { get; set; }

    /// <summary>
    /// ФИО.
    /// </summary>
    [MapField("fio")]
    public string Name { get; set; }

    /// <summary>
    /// Факультет.
    /// </summary>
    public string Department { get; set; }

    /// <summary>
    /// Группа.
    /// </summary>
    [MapField("grp")]
    public string Group { get; set; }

    /// <summary>
    /// Адрес.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Телефон.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Статус.
    /// </summary>
    public string Status { get; set; }
  }

  /// <summary>
  /// Книга научного фонда.
  /// </summary>
  [TableName("Podsob")]
  [UsedImplicitly]
  public class Podsob
  {
    /// <summary>
    /// Инвентарный номер.
    /// </summary>
    [PrimaryKey]
    [MapField ("INVENT")]
    public int Inventory { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Читательский билет.
    /// </summary>
    [MapField ("CHB")]
    public string Ticket { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Предполагаемый срок возврата.
    /// </summary>
    [MapField ("Srok")]
    public DateTime Deadline { get; [UsedImplicitly] set; }
  }

  /// <summary>
  /// Книга учебного фонда.
  /// </summary>
  [TableName("Uchtrans")]
  [UsedImplicitly]
  public class Uch
  {
    /// <summary>
    /// Штрих-код.
    /// </summary>
    [PrimaryKey]
    public string Barcode { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Радиометка.
    /// </summary>
    [UsedImplicitly]
    public string Rfid { get; set; }
    
    /// <summary>
    /// Номер карточки комплектования.
    /// </summary>
    [MapField("Cardnum")]
    public string Card { get; [UsedImplicitly] set; }

    /// <summary>
    /// Читательский билет.
    /// </summary>
    [MapField("CHB")]
    public string Ticket { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Предполагаемый срок возврата.
    /// </summary>
    [MapField("Srok")]
    public DateTime Deadline { get; [UsedImplicitly] set; }
  }

  /// <summary>
  /// Книга с художественного абонемента.
  /// </summary>
  [TableName("Hudtrans")]
  [UsedImplicitly]
  public class Hudo
  {
    [PrimaryKey]
    [MapField("Invnum")]
    public string Inventory { get; [UsedImplicitly] set; }
    
    [MapField("Chb")]
    public string Ticket { get; [UsedImplicitly] set; }
    
    [MapField("Srok")]
    public DateTime Deadline { get; [UsedImplicitly] set; }
  }

  /// <summary>
  /// Журнал в целом.
  /// </summary>
  [TableName("Magazines")]
  [UsedImplicitly]
  public class Magazine
  {
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Заглавие журнала.
    /// </summary>
    public string Title { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Периодичность (номеров в год).
    /// </summary>
    [UsedImplicitly]
    public int Period { get; set; }
  }
  
  /// <summary>
  /// Номер журнала.
  /// </summary>
  [TableName("Magtrans")]
  [UsedImplicitly]
  public class MagazineNumber
  {
    /// <summary>
    /// Штрих-код.
    /// </summary>
    [PrimaryKey]
    public string Barcode { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Читательский билет.
    /// </summary>
    [MapField ("Chb")]
    public string Ticket { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Идентификатор журнала.
    /// </summary>
    [MapField ("Magazine")]
    public int MagazineId { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Год издания.
    /// </summary>
    public int Year { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Том (может отсутствовать).
    /// </summary>
    public int Volume { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Номер выпуска.
    /// </summary>
    public int Number { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Предполагаемый срок возврата.
    /// </summary>
    [MapField("Srok")]
    public DateTime Deadline { get; [UsedImplicitly] set; }
  }
  
  /// <summary>
  /// Информация об одной задолженности.
  /// </summary>
  public class BookDebt
  {
    /// <summary>
    /// Инвентарный номер (или штрих-код) книги.
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Предполагаемый срок возврата.
    /// </summary>
    public DateTime Deadline { get; set; }
    
    /// <summary>
    /// Библиографическое описание.
    /// </summary>
    public string Description { get; set; }
  };

  /// <summary>
  /// Информация о читателе.
  /// </summary>
  [TableName("readers")]
  [UsedImplicitly]
  public class Reader
  {
    /// <summary>
    /// Идентификатор в базе данных книговыдачи.
    /// </summary>
    [PrimaryKey]
    [UsedImplicitly]
    public int Id { get; set; }
    
    /// <summary>
    /// Читательский билет.
    /// </summary>
    [UsedImplicitly]
    public string Ticket { get; set; }
    
    /// <summary>
    /// ФИО.
    /// </summary>
    [UsedImplicitly]
    public string Name { get; set; }
    
    /// <summary>
    /// Электронная почта.
    /// </summary>
    [UsedImplicitly]
    public string Mail { get; set; }
    
    /// <summary>
    /// Вечный должник.
    /// </summary>
    [UsedImplicitly]
    public bool Debtor { get; set; }
    
    /// <summary>
    /// Вечный должник.
    /// </summary>
    [UsedImplicitly]
    public bool Everlasting { get; set; }
    
    /// <summary>
    /// Ругачка. 
    /// </summary>
    [UsedImplicitly]
    public string Alert { get; set; }
    
    /// <summary>
    /// Факультет.
    /// </summary>
    [UsedImplicitly]
    public string Department { get; set; }
    
    /// <summary>
    /// Группа.
    /// </summary>
    public string Group { get; [UsedImplicitly] set; }
  }
  
  /// <summary>
  /// Информация о задолжнике.
  /// </summary>
  [UsedImplicitly]
  public class Debtor
  {
    /// <summary>
    /// Читательский билет.
    /// </summary>
    public string Ticket { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// ФИО. 
    /// </summary>
    public string Name { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Электронная почта.
    /// </summary>
    public string Mail { get; set; }

    /// <summary>
    /// Вечный должник.
    /// </summary>
    public bool debtor { get; set; }

    /// <summary>
    /// Вечный должник.
    /// </summary>
    [UsedImplicitly]
    public bool Everlasting { get; set; }
    
    /// <summary>
    /// Ругачка. 
    /// </summary>
    public string Alert { get; set; }
    
    /// <summary>
    /// Факультет.
    /// </summary>
    public string Department { get; [UsedImplicitly] set; }
    
    /// <summary>
    /// Группа.
    /// </summary>
    public string Group { get; [UsedImplicitly] set; }

    /// <summary>
    /// Идентификатор, присвоенный отделом кадров.
    /// </summary>
    public int IstuID { get; set; }
    
    /// <summary>
    /// Задолженный книги.
    /// </summary>
    public readonly List<BookDebt> Books = new List<BookDebt>();
  }
  
  internal static class Program
  {
    private static string descriptionFormat;
    private static string[] graduatedGroups;
    private static DbManager sql;
    private static BLToolkit.Data.Linq.Table<Student> students;
    private static BLToolkit.Data.Linq.Table<Reader> readers;
    private static BLToolkit.Data.Linq.Table<Podsob> podsob;
    private static BLToolkit.Data.Linq.Table<Uch> uch;
    private static BLToolkit.Data.Linq.Table<Hudo> hudo;
    private static BLToolkit.Data.Linq.Table<Magazine> magazines;
    private static BLToolkit.Data.Linq.Table<MagazineNumber> magazineNumbers;
    private static IrbisConnection irbis;
    private static Student[] identifiedStudents;
    private static readonly List<Debtor> debtors = new List<Debtor> ();
    private static readonly Dictionary<string, string> uchDescriptions 
          = new Dictionary<string, string>();
    private static readonly Dictionary<int, string> magazineDescriptions 
          = new Dictionary<int, string>();

    /// <summary>
    /// Группа выпускается в этом году или нет?
    /// </summary>
    static bool MatchGroup (string group)
    {
      group = group.ToUpperInvariant();
      foreach (var one in graduatedGroups)
      {
        if (group.StartsWith (one.ToUpperInvariant()))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Считываем список выпускающихся групп.
    /// </summary>
    private static void ReadGroupList()
    {
      Console.Write("Reading groups... ");
      graduatedGroups = File.ReadAllLines("groups.txt")
        .Select(line => line.Trim())
        .Where(line => !string.IsNullOrEmpty(line))
        .ToArray();
      Console.Write("{0} ", graduatedGroups.Length);
      Console.WriteLine("done");
      if (graduatedGroups.Length == 0)
      {
        throw new Exception("No groups found");
      }
    }

    /// <summary>
    /// Подключение к базе MSSQL.
    /// </summary>
    private static void ConnectToMssql()
    {
      Console.Write("Connecting to MSSQL... ");
      sql = new DbManager("MSSQL");
      Console.WriteLine("done");
    }

    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    private static void ConnectToIrbis()
    {
      Console.Write("Connecting to IRBIS... ");
      var connectionString = ConfigurationUtility.GetString("ConnectionString.IRBIS");
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new Exception("No IRBIS connection string specified");
      }
      irbis = new IrbisConnection(connectionString);
      Console.WriteLine("done");
    }

    /// <summary>
    /// Забираем данные из базы отдела кадров
    /// </summary>
    private static void CollectIdentified()
    {
      var istuDatabase = ConfigurationUtility.GetString
        (
            "istu",
            "istu"
        );
      var currentDatabase = sql
        .SetCommand("select DB_NAME()")
        .ExecuteScalar<string>();
      sql.SetCommand("use [" + istuDatabase + "]").ExecuteNonQuery();
      students = sql.GetTable<Student>();
      identifiedStudents = students.ToArray();
      sql.SetCommand("use [" + currentDatabase + "]").ExecuteNonQuery();
      Console.WriteLine("Total identified: {0}", identifiedStudents.Length);
    }

    /// <summary>
    /// Собираем сведения о задолжниках.
    /// </summary>
    private static void CollectDebtors()
    {
      // Алиасы для таблиц.
      readers = sql.GetTable<Reader>();
      podsob = sql.GetTable<Podsob>();
      uch = sql.GetTable<Uch>();
      hudo = sql.GetTable<Hudo>();
      magazines = sql.GetTable<Magazine>();
      magazineNumbers = sql.GetTable<MagazineNumber>();
      
      // Сначала вычитываем всех студентов вообще
      Reader[] allStudents =readers
        .Where(r => r.Group != null)
        .ToArray();
      Console.WriteLine ("Total students: {0}", allStudents.Length);

      // Отбираем только тех, кто в выпускающейся группе.
      var graduates =
        (
          from r in allStudents
          where MatchGroup (r.Group)
          select r
        )
        .ToArray();
      Console.WriteLine ("Graduates: {0}", graduates.Length);

      // Для каждого выпускника собираем задолженность.
      Console.Write("Processing graduates... ");
      int index = 0;
      foreach (var graduate in graduates)
      {
        if (index % 100 == 0)
        {
          Console.Write("{0} ", index);
        }

        ++index;
        ProcessGraduate(graduate);
      }
      Console.WriteLine("done");
    }

    /// <summary>
    /// Поиск и расформатирование записи.
    /// </summary>
    private static string FindAndFormat(string searchExpression)
    {
      var found = irbis.SearchFormat(searchExpression, descriptionFormat);
      if (found.Length != 0)
      {
        string result = found[0].Text;
        if (!string.IsNullOrEmpty(result))
        {
          result = result.Trim();
        }

        if (!string.IsNullOrEmpty(result))
        {
          return result;
        }
      }

      return "??? " + searchExpression;
    }

    /// <summary>
    /// Получение библиографического описания по инвентарному номеру
    /// книги научного фонда.
    /// </summary>
    private static string PodsobDescription(int inventory)
    {
      var searchExpression = string.Format("\"IN={0}\"", inventory);
      return FindAndFormat(searchExpression);
    }

    /// <summary>
    /// Поиск книг научного фонда, числящихся за указанным читателем.
    /// </summary>
    private static void PodsobBooks(Debtor debtor)
    {
      var books = podsob
        .Where(b => b.Ticket == debtor.Ticket)
        .ToArray();
      
      foreach (var book in books)
      {
        var debt = new BookDebt
        {
          Number = book.Inventory.ToInvariantString(),
          Description = PodsobDescription(book.Inventory),
          Deadline = book.Deadline
        };
        debtor.Books.Add(debt);
      }
    }

    /// <summary>
    /// Получение библиографического описания для книги учебного фонда.
    /// </summary>
    private static string UchDescription(string cardNumber)
    {
      if (uchDescriptions.TryGetValue(cardNumber, out var result))
      {
        return result;
      }
      var searchExpression = string.Format("\"NS={0}\"", cardNumber);
      result = FindAndFormat(searchExpression);
      uchDescriptions.Add(cardNumber, result);
      return result;
    }
    
    /// <summary>
    /// Поиск книг учебного фонда, числящихся за указанным читателем.
    /// </summary>
    private static void UchBooks(Debtor debtor)
    {
      var books = uch
        .Where(b => b.Ticket == debtor.Ticket)
        .ToArray();

      foreach (var book in books)
      {
        var debt = new BookDebt
        {
          Number = book.Barcode,
          Description = UchDescription(book.Card),
          Deadline = book.Deadline
        };
        debtor.Books.Add(debt);
      }
    }

    /// <summary>
    /// Получение библиографического описания художественной книги.
    /// </summary>
    private static string HudoDescription(string inventory)
    {
      try
      {
        irbis.PushDatabase("HUDO");
        var searchExpression = string.Format("\"IN={0}\"", inventory);
        return FindAndFormat(searchExpression);
      }
      finally
      {
        irbis.PopDatabase();
      }
    }

    /// <summary>
    /// Поиск художественных книг, числящихся за указанным читателем.
    /// </summary>
    private static void HudoBooks(Debtor debtor)
    {
      var books = hudo
        .Where(b => b.Ticket == debtor.Ticket)
        .ToArray();

      foreach (var book in books)
      {
        var debt = new BookDebt
        {
          Number = book.Inventory,
          Description = HudoDescription(book.Inventory),
          Deadline = book.Deadline
        };
        debtor.Books.Add(debt);
      }
    }

    /// <summary>
    /// Получение библиографического описания для указанного номера журнала.
    /// </summary>
    private static string MagazineDescription(MagazineNumber number)
    {
      if (!magazineDescriptions.TryGetValue(number.MagazineId, out var result))
      {
        result = magazines
          .Where(x => x.Id == number.MagazineId)
          .Select(x => x.Title)
          .FirstOrDefault();
        if (string.IsNullOrEmpty(result))
        {
          result = "???";
        }
        magazineDescriptions.Add(number.MagazineId, result);
      }

      result += string.Format(" {0} год", number.Year);
      if (number.Volume != 0)
      {
        result += string.Format(", том {0}", number.Volume);
      }

      if (number.Number != 0)
      {
        result += string.Format(", № {0}", number.Number);
      }
      
      return "Журнал " + result;
    }

    /// <summary>
    /// Поиск журналов, числящихся за указанным читателем.
    /// </summary>
    private static void Magazines(Debtor debtor)
    {
      var books = magazineNumbers
        .Where(b => b.Ticket == debtor.Ticket)
        .ToArray();

      foreach (var book in books)
      {
        var debt = new BookDebt
        {
          Number = book.Barcode,
          Description = MagazineDescription(book),
          Deadline = book.Deadline
        };
        debtor.Books.Add(debt);
      }
    }

    /// <summary>
    /// Идентификация задолжника.
    /// </summary>
    private static void IdentifyDebtor(Debtor debtor)
    {
        var match = identifiedStudents
            .Where(student => student.Name.SameString(debtor.Name))
            .ToArray();
        if (match.Length == 1)
        {
            debtor.IstuID = match[0].Id;
        }
    }

    /// <summary>
    /// Сбор данных о данном читателе.
    /// </summary>
    private static void ProcessGraduate(Reader graduate)
    {
      var debtor = Map.ObjectToObject<Debtor>(graduate);

      PodsobBooks(debtor);
      UchBooks(debtor);
      HudoBooks(debtor);
      Magazines(debtor);

      if (debtor.Books.Count != 0
        || debtor.debtor
        || (!string.IsNullOrEmpty (debtor.Alert) 
            &&  debtor.Alert.ToLowerInvariant().Contains ("долг")))
      {
        IdentifyDebtor(debtor);
        debtors.Add(debtor);
      }
    }

    /// <summary>
    /// Вывод списка задолжников.
    /// </summary>
    private static void DumpDebtors()
    {
      Console.WriteLine("Debtors: {0}", debtors.Count);
      var totalBooks = debtors.Sum(d => d.Books.Count);
      Console.WriteLine("Books: {0}", totalBooks);
      
      ExcelOutput excel = new ExcelOutput();
      excel.SetColumnWidth(3, 3, 30, 25, 100);

      // в порядке алфавита факультетов
      var byDepartment = debtors.GroupBy(d => d.Department)
        .OrderBy(g => g.Key);
      foreach (var department in byDepartment)
      {
        Console.Write("{0} ({1}) ... ", department.Key, department.Count());
        excel.WriteLine();
        excel.MergeCells(0,4);
        excel.WriteLine(department.Key).Bold().FontSize(24).WrapText()
          .Background(Color.LightGray);
        excel.WriteLine();

        // в порядке алфавита групп
        var byGroup = department.GroupBy(g => g.Group)
          .OrderBy(g => g.Key);
        foreach (var group in byGroup)
        {
          Console.Write("{0} ({1}) ", group.Key, group.Count());
          excel.WriteLine("Группа: " + group.Key).Bold().FontSize(14).TextColor(Color.Blue);
          excel.WriteLine();

          // в порядке алфавита студентов
          var ordered = group.OrderBy(g => g.Name);
          foreach (var debtor in ordered)
          {
            // var goodToShow = debtor.debtor || debtor.Everlasting
            //   || debtor.Books.Count != 0
            //   || (!string.IsNullOrEmpty(debtor.Alert)
            //     && debtor.Alert.ToLowerInvariant().Contains("долг"));
            //
            // if (!goodToShow)
            // {
            //   continue;
            // }
            
            var nameWithEmail = debtor.Name;
            if (!string.IsNullOrEmpty(debtor.Mail))
            {
              nameWithEmail += (" <" + debtor.Mail + ">");
            }
            excel.WriteCell(1, nameWithEmail).FontSize(12).Bold();
            excel.WriteLine();

            var identifier = debtor.IstuID == 0 
                ? "--" 
                : debtor.IstuID.ToInvariantString();
            excel.WriteCell(2, identifier).FontSize(12).RightJustify();
            excel.WriteLine();

            if (debtor.debtor || debtor.Everlasting)
            {
              excel.WriteCell(1, "Вечный должник");
              excel.WriteLine();
            }

            if (!string.IsNullOrEmpty(debtor.Alert) 
                && debtor.Alert.ToLowerInvariant().Contains ("долг"))
            {
              excel.WriteCell(1, debtor.Alert);
              excel.WriteLine();
            }

            // в порядке алфавита библиографических описаний
            // скорее всего -- сортировка по авторам
            var books = debtor.Books.OrderBy(b => b.Description);
            foreach (var book in books)
            {
              excel.WriteCell(2, book.Number).SetBorders().Center();
              excel.WriteCell(3, book.Deadline).SetBorders().Center();
              excel.WriteCell(4, book.Description).SetBorders().WrapText();
              excel.WriteLine();
            }
            excel.WriteLine();
          }
          
          excel.PageBreak();
        }
        
        Console.WriteLine("done");
      }
      
      excel.Save("corona.xlsx");
    }

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    public static void Main()
    {
      try
      {
        descriptionFormat = ConfigurationUtility.GetString("format", "@brief");
        ReadGroupList();
        ConnectToMssql();
        
        using (sql)
        {
          ConnectToIrbis();
          using (irbis)
          {
            CollectIdentified();
            CollectDebtors();
            Console.WriteLine("Disconnect from IRBIS");
          }

          Console.WriteLine("Disconnect from SQL");
        }
        
        DumpDebtors();
        
        Console.WriteLine("ALL DONE");
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
  }
}
