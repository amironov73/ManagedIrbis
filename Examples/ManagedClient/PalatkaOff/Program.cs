using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;

internal class Program
{
  private static IrbisConnection connection;
  private static string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=C;";
  
  public static void Main()
  {
    try
    {
      using (connection = new IrbisConnection(connectionString))
      {
        // int[] found = connection.Search("HD=n$ * MHR=Ф403");
        int[] found = { 185505, 185770, 194000, 193830, 193944, 185649, 
          190188, 185556, 189272, 194943, 194951 };
        
        BatchRecordReader batch = new BatchRecordReader(connection, connection.Database, 500, found);
        foreach (MarcRecord record in batch)
        {
          // ExemplarInfo[] exemplars = ExemplarInfo.Parse(record);
          // if (exemplars.Length == 1 
          //    && exemplars[0].Barcode.IsBlank()
          //    && (exemplars[0].Number ?? string.Empty).Length < 2)
          {
            Console.Write($" {record.Mfn} ");

            RecordStatus saveStatus = record.Status;
            record.Deleted = true;
            connection.WriteRecord(record);

            record.Status = saveStatus;
            record.Mfn = 0;
            record.Version = 0;
            record.Database = "KARMAN";
            connection.WriteRecord(record);
          }
          //else
          //{
          //  Console.Write('.');
          //}
        }
      }
    }
    catch (Exception exception)
    {
      Console.WriteLine(exception);
    }
  }
}