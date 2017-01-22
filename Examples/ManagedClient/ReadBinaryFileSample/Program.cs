using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

class Program
{
    static void Main()
    {
        const string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=IMAGE;arm=C;";

        try
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(connectionString);
                client.Connect();

                const string fileName = @"CARDS\АЙЗИКОВИЧ - АКАДЕМИЯ ГРАЖДАНСКОЙ АВИАЦИИ\Q0001.jpg";

                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.FullText,
                        "IMAGE",
                        fileName
                    );

                byte[] bytes = client.ReadBinaryFile(specification);

                if (ReferenceEquals(bytes, null))
                {
                    Console.WriteLine("no data read");
                }
                else
                {
                    Console.WriteLine("bytes read: {0}", bytes.Length);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}
