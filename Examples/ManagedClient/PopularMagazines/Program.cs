using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;
using ManagedIrbis.Magazines;

using CM=System.Configuration.ConfigurationManager;

namespace PopularMagazines
{
    class Program
    {
        private static int CountExemplars
            (
                MagazineIssueInfo issue
            )
        {
            return ReferenceEquals(issue.Exemplars, null)
                ? 0
                : issue.Exemplars.Length;
        }

        static void Main()
        {
            try
            {
                using (IrbisConnection Connection = new IrbisConnection())
                {
                    string connectionString 
                        = CM.AppSettings["connectionString"];
                    Connection.ParseConnectionString(connectionString);
                    Connection.Connect();

                    Console.WriteLine("Loading magazines...");

                    MagazineManager Manager = new MagazineManager(Connection);
                    MagazineInfo[] Magazines = Manager.GetAllMagazines();

                    Console.WriteLine
                        (
                            "Magazines loaded: {0}",
                            Magazines.Length
                        );

                    foreach (MagazineInfo magazine in Magazines)
                    {
                        MagazineIssueInfo[] issues = Manager.GetIssues(magazine);
                        string kind = magazine.MagazineKind == "a"
                            ? "Журнал"
                            : "Газета";
                        int exemplars = issues.Sum(issue => CountExemplars(issue));
                        int loans = issues.Sum(issue => issue.LoanCount);
                        Console.WriteLine
                            (
                                "{0}\t{1}\t{2}\t{3}",
                                magazine.ExtendedTitle,
                                kind,
                                exemplars,
                                loans
                            );
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
