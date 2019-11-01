// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using AM;
using AM.Configuration;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/// <summary>
/// Сервис для Бери-Читай.
/// </summary>
public class Beri : IHttpHandler
{
    private const string StatusPrefix = "BERI=";
    private const string FreeBook = "0";

    private static IrbisConnection GetConnection()
    {
        string connectionString = ConfigurationUtility.GetString("connectionString", "");
        return new IrbisConnection(connectionString);
    }

    public void ProcessRequest(HttpContext context)
    {
        // Для отладки разрешаем доступ к сервису с посторонних ресурсов.
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET");
        context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");

        var query = context.Request.Url.Query;
        if (query.StartsWith("?"))
        {
            query = query.Substring(1);
        }

        if (query.StartsWith("cover"))
        {
            Cover(context);
            return;
        }
        
        context.Response.ContentType = "application/json";

        object result = null;

        try
        {
            if (query == "random")
            {
                result = RandomBooks();
            }
            else if (query == "all")
            {
                result = AllBooks();
            }
            else if (query.StartsWith("read"))
            {
                result = ReadBooks(context);
            }
            else if (query.StartsWith("order"))
            {
                result = OrderBooks(context);
            }
            else if (query.StartsWith("count"))
            {
                result = CountBooks();
            }
            else if (query.StartsWith("search"))
            {
                var parms = context.Request.Params;
                var word = parms["search"];
                result = SearchBooks(word);
            }
        }
        catch
        {
            result = null;
        }

        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    /// <summary>
    /// Получение массива MFN из строки запроса.
    /// </summary>
    private int[] ExtractMfn(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new int[0];
        }

        var result = text.Split
            (
                new[] {',', ' ', ';'},
                StringSplitOptions.RemoveEmptyEntries
            )
            .Select(chunk => chunk.SafeToInt32())
            .Where(one => one > 0)
            .Distinct()
            .ToArray();

        return result;
    }

    /// <summary>
    /// Поиск читателя по номеру читательского
    /// и email (либо номеру телефона).
    /// </summary>
    /// <returns>Если читатель не найден,
    /// возвращает <c>null</c>.</returns>
    [CanBeNull]
    private ReaderInfo FindReader
        (
            IrbisConnection connection,
            string ticket,
            string email
        )
    {
        try
        {
            connection.PushDatabase("RDR");
            var manager = new ReaderManager(connection);
            var result = manager.GetReader(ticket);
            if (ReferenceEquals(result, null))
            {
                return null;
            }

            // должно совпадать с одним из двух полей:
            // Email или HomePhone
            if (!email.SameString(result.Email)
                && !email.SameString(result.HomePhone))
            {
                return null;
            }

            return result;
        }
        finally
        {
            connection.PopDatabase();
        }
    }

    /// <summary>
    /// Заказ книг.
    /// </summary>
    /// <returns>
    /// Используются параметры:
    /// ticket -- номер читательского
    /// email -- почта либо телефон
    /// order -- MFN заказываемых книг
    /// </returns>
    private object OrderBooks(HttpContext context)
    {
        var ticket = context.Request.Params["ticket"];
        if (string.IsNullOrEmpty(ticket))
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Не задан читательский билет"
            };
        }

        var email = context.Request.Params["email"];
        if (string.IsNullOrEmpty(email))
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Не задан e-mail или телефон"
            };
        }

        var mfns = ExtractMfn(context.Request.Params["order"]);
        if (mfns.Length == 0)
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Не заданы книги для заказа"
            };
        }

        using (var connection = GetConnection())
        {
            var maxMfn = connection.GetMaxMfn();
            mfns = mfns.Where(item => item <= maxMfn).ToArray();
            if (mfns.Length == 0)
            {
                return new OrderResult
                {
                    Ok = false,
                    Message = "Все MFN вне пределов базы"
                };
            }

            var reader = FindReader(connection, ticket, email);
            if (ReferenceEquals(reader, null))
            {
                return new OrderResult
                {
                    Ok = false,
                    Message = "Неверные учётные данные"
                };
            }

            // Тот, кто умеет размещать заказы
            var beriMan = new BeriManager(connection);

            // Читатель может быть лишен права пользования библиотекой
            if (!beriMan.IsReaderEnabled(reader))
            {
                return new OrderResult
                {
                    Ok = false,
                    Message = "Читатель лишён права пользования библиотекой"
                };
            }
            
            // Собственно заказ книг
            foreach (var mfn in mfns)
            {
                if (!beriMan.CreateBooking(mfn, reader))
                {
                    return new OrderResult
                    {
                        Ok = false,
                        Message = $"Ошибка при заказе книги с MFN {mfn}"
                    };
                }
            } // foreach
        } // using

        return new OrderResult
        {
            Ok = true,
            Message = "Заказ успешно размещён"
        };
    }

    private object AllBooks()
    {
        using (var connection = GetConnection())
        {
            var books = connection.SearchFormat
                (
                    StatusPrefix + FreeBook,
                    "@brief"
                )
                .Select(book => new BookInfo
                {
                    Mfn = book.Mfn,
                    Description = book.Text,
                    Selected = false
                })
                .OrderBy(book => book.Description)
                .ToArray();
            return books;
        }
    }

    /// <summary>
    /// Выдача книг по MFN.
    /// </summary>
    private object ReadBooks(HttpContext context)
    {
        var mfns = ExtractMfn(context.Request.Params["read"]);
        if (mfns.Length == 0)
        {
            return new BookInfo[0];
        }

        using (var connection = GetConnection())
        {
            var maxMfn = connection.GetMaxMfn();
            mfns = mfns.Where(item => item < maxMfn).ToArray();
            if (mfns.Length == 0)
            {
                return new BookInfo[0];
            }

            var result = connection.FormatRecords
                (
                    connection.Database,
                    "@brief",
                    mfns
                )
                .Zip(mfns, (first, second) => new BookInfo
                {
                    Selected = false,
                    Mfn = second,
                    Description = first
                })
                .ToArray();
            return result;
        }
    }

    /// <summary>
    /// Обложка для книги.
    /// </summary>
    private void Cover(HttpContext context)
    {
        var filename = "img/nophoto.jpg";
        var index = context.Request.Params["cover"];
        if (string.IsNullOrEmpty(index))
        {
            goto FINISH;
        }

        index = Path.GetFileNameWithoutExtension(index);
        if (string.IsNullOrEmpty(index))
        {
            goto FINISH;
        }

        var picturePath = CM.AppSettings["picturePath"];
        if (!Directory.Exists(picturePath))
        {
            goto FINISH;
        }

        var candidate = Path.Combine(picturePath, index + ".jpg");
        if (!File.Exists(candidate))
        {
            goto FINISH;
        }

        filename = candidate;

        FINISH:
        context.Response.ContentType = "image/jpeg";
        context.Response.WriteFile(filename);
    }

    /// <summary>
    /// Пять случайных книг.
    /// </summary>
    private object RandomBooks()
    {
        var result = new List<BookInfo>();
        using (var connection = GetConnection())
        {
            var found = connection.SearchFormat
                (
                    StatusPrefix + FreeBook,
                    "@brief"
                )
                .ToList();
            var random = new Random();
            for (var i = 0; i < 5 && found.Count != 0; i++)
            {
                var index = random.Next(found.Count);
                var one = found[index];
                found.RemoveAt(index);
                var book = new BookInfo
                {
                    Selected = false,
                    Mfn = one.Mfn,
                    Description = one.Text
                };
                result.Add(book);
            }
        }
        return result.ToArray();
    }

    /// <summary>
    /// Поиск книг по ключевому слову
    /// (не более 100 штук).
    /// </summary>
    private object SearchBooks(string keyword)
    {
        using (var connection = GetConnection())
        {
            var found = connection.SearchFormat
                    (
                        $"\"K={keyword}$\"",
                        "@brief"
                    )
                .Take(100)
                .Select(book => new BookInfo
                    {
                        Selected = false,
                        Mfn = book.Mfn,
                        Description = book.Text
                    })
                .ToArray();
            return found;
        }
    }

    /// <summary>
    /// Общее количество книг, доступных для заказа.
    /// </summary>
    private object CountBooks()
    {
        using (var connection = GetConnection())
        {
            var count = connection.SearchCount(StatusPrefix + FreeBook);
            return count;
        }
    }

    public bool IsReusable => false;
}
