using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;
using ManagedIrbis.Server;

namespace UnitTests.Experiments
{
    [TestClass]
    public class ServerExperiments
        : Common.CommonUnitTest
    {
        [NotNull]
        private IrbisServerEngine _GetEngine()
        {
            string serverRootPath = Irbis64RootPath;

            string fileName = Path.Combine(serverRootPath, "irbis_server.ini");
            IniFile simpleIni = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile serverIni = new ServerIniFile(simpleIni);
            IrbisServerEngine engine = new IrbisServerEngine(serverIni, serverRootPath);
            Assert.IsNotNull(engine.IniFile);
            Assert.IsNotNull(engine.ClientIni);
            Assert.IsNotNull(engine.Contexts);
            Assert.IsNotNull(engine.DataPath);
            Assert.IsNotNull(engine.Listener);
            Assert.IsNotNull(engine.Mapper);
            Assert.IsNotNull(engine.StopSignal);
            Assert.IsNotNull(engine.SystemPath);
            Assert.IsNotNull(engine.Users);
            Assert.IsNotNull(engine.WorkDir);
            Assert.IsNotNull(engine.Workers);

            Assert.AreEqual(2, engine.Users.Length);
            Assert.AreEqual("librarian", engine.Users[0].Name);
            Assert.AreEqual("secret", engine.Users[0].Password);

            return engine;
        }

        private Exception _RunAction
            (
                [NotNull] Action<IrbisConnection> action
            )
        {
            Exception result = null;

            using (IrbisServerEngine engine = _GetEngine())
            {
                Task actionTask = new Task(() =>
                {
                    try
                    {
                        Task.Delay(100).Wait();
                        IrbisConnection connection = new IrbisConnection
                        {
                            Host = "127.0.0.1",
                            Port = 6666,
                            Username = "librarian",
                            Password = "secret",
                            Workstation = IrbisWorkstation.Cataloger
                        };
                        connection.Connect();

                        action(connection);

                        connection.Dispose();
                        engine.StopSignal.Set();
                    }
                    catch (Exception ex)
                    {
                        engine.StopSignal.Set();
                        result = ex;
                    }
                });
                try
                {
                    actionTask.Start();
                    engine.MainLoop();
                    actionTask.Wait();

                    if (!ReferenceEquals(result, null))
                    {
                        return result;
                    }

                    Assert.AreEqual(0, engine.Contexts.Count);
                }
                catch (Exception ex)
                {
                    return ex;
                }

                return result;
            }
        }

        [TestMethod]
        public void Server_NoOp_1()
        {
            Exception ex = _RunAction(connection =>
            {
                connection.NoOp();
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_GetMaxMfn_1()
        {
            Exception ex = _RunAction(connection =>
            {
                int maxMfn = connection.GetMaxMfn("IBIS");
                Assert.AreEqual(332, maxMfn);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadRecord_1()
        {
            Exception ex = _RunAction(connection =>
            {
                MarcRecord record = connection.ReadRecord("IBIS", 1, false, null);
                Assert.AreEqual(100, record.Fields.Count);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_GetServerVersion_1()
        {
            Exception ex = _RunAction(connection =>
            {
                IrbisVersion expected = ServerUtility.GetServerVersion();
                IrbisVersion actual = connection.GetServerVersion();
                Assert.AreEqual(expected.Organization, actual.Organization);
                Assert.AreEqual(expected.Version, actual.Version);
                Assert.AreEqual(1, actual.ConnectedClients);
                Assert.AreEqual(expected.MaxClients, actual.MaxClients);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_UpdateIniFile_1()
        {
            Exception ex = _RunAction(connection =>
            {
                List<string> lines = new List<string>();
                lines.Add("[MIRON]");
                lines.Add(string.Format("LastAccess={0}", DateTime.Now));
                connection.UpdateIniFile(lines.ToArray());
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadFile_1()
        {
            Exception ex = _RunAction(connection =>
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "dumb.fst"
                    );
                string expected = "201 0 (v200 /)\r\n";
                string actual = connection.ReadTextFile(specification);
                Assert.AreEqual(expected, actual);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadFile_2()
        {
            Exception ex = _RunAction(connection =>
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "dumb.fst"
                    );
                byte[] expected = IrbisEncoding.Ansi.GetBytes("201 0 (v200 /)\r\n");
                byte[] actual = connection.ReadBinaryFile(specification);
                CollectionAssert.AreEqual(expected, actual);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ListFiles_1()
        {
            Exception ex = _RunAction(connection =>
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "*.fst"
                    );
                string[] expected = { "dumb.fst", "empty.fst", "empty2.fst", "ibis.fst" };
                string[] actual = connection.ListFiles(specification);
                CollectionAssert.AreEqual(expected, actual);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_FormatRecord_1()
        {
            Exception ex = _RunAction(connection =>
            {
                string expected = "Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                string actual = connection.FormatRecord
                    (
                        "v200^a, | : |v200^e, | / |v200^f ",
                        1
                    );
                Assert.AreEqual(expected, actual);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_FormatRecord_2()
        {
            Exception ex = _RunAction(connection =>
            {
                string[] expected =
                {
                    "Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]",
                    "Энергетическая и информационная электроника : Сб. / ред. В. А. Лабунцов [и др.]"
                };
                string[] actual = connection.FormatRecords
                    (
                        "IBIS",
                        "v200^a, | : |v200^e, | / |v200^f ",
                        new [] { 1, 3 }
                    );
                CollectionAssert.AreEqual(expected, actual);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_FormatRecord_3()
        {
            Exception ex = _RunAction(connection =>
            {
                MarcRecord record = new MarcRecord();
                record.AddField(new RecordField(200, new[]
                {
                    new SubField('a', "Куда пойти учиться?"),
                    new SubField('e', "Информ. - реклам. справ"),
                    new SubField('f', "З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]")
                }));

                string expected = "Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                string actual = connection.FormatRecord
                    (
                        "v200^a, | : |v200^e, | / |v200^f ",
                        record
                    );
                Assert.AreEqual(expected, actual);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_Search_1()
        {
            Exception ex = _RunAction(connection =>
            {
                int[] found = connection.Search("K=БИЗНЕС");
                Assert.AreEqual(1, found.Length);
                Assert.AreEqual(192, found[0]);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_Search_2()
        {
            Exception ex = _RunAction(connection =>
            {
                int[] found = connection.Search("K=БИЗНЕС$");
                Assert.AreEqual(2, found.Length);
                Assert.AreEqual(192, found[0]);
                Assert.AreEqual(63, found[1]);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_Search_3()
        {
            Exception ex = _RunAction(connection =>
            {
                int found = connection.SearchCount("K=БИЗНЕС$");
                Assert.AreEqual(2, found);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_Search_4()
        {
            Exception ex = _RunAction(connection =>
            {
                FoundItem[] found = connection.SearchFormat("K=БИЗНЕС$", "v200^a");
                Assert.AreEqual(2, found.Length);
                Assert.AreEqual(192, found[0].Mfn);
                Assert.AreEqual("Информационные технологии для менеджеров", found[0].Text);
                Assert.AreEqual(63, found[1].Mfn);
                Assert.AreEqual("Английский для бизнесменов", found[1].Text);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_Search_5()
        {
            Exception ex = _RunAction(connection =>
            {
                int[] found = connection.Search("\"K=НЕТ ТАКОГО СЛОВА\"");
                Assert.AreEqual(0, found.Length);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadTerms_1()
        {
            Exception ex = _RunAction(connection =>
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    StartTerm = "K=АГРАРНОЕ",
                    NumberOfTerms = 10
                };
                TermInfo[] terms = connection.ReadTerms(parameters);
                Assert.AreEqual(10, terms.Length);
                Assert.AreEqual(8, terms[0].Count);
                Assert.AreEqual("K=АГРАРНОЕ", terms[0].Text);
                Assert.AreEqual(2, terms[1].Count);
                Assert.AreEqual("K=АГРАРНОЕ ПРАВО", terms[1].Text);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadTerms_2()
        {
            Exception ex = _RunAction(connection =>
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    StartTerm = "K=АГРАР",
                    NumberOfTerms = 10
                };
                TermInfo[] terms = connection.ReadTerms(parameters);
                Assert.AreEqual(10, terms.Length);
                Assert.AreEqual(8, terms[0].Count);
                Assert.AreEqual("K=АГРАРНОЕ", terms[0].Text);
                Assert.AreEqual(2, terms[1].Count);
                Assert.AreEqual("K=АГРАРНОЕ ПРАВО", terms[1].Text);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadPostings_1()
        {
            Exception ex = _RunAction(connection =>
            {
                PostingParameters parameters = new PostingParameters
                {
                    Database = "IBIS",
                    Term = "K=АГРАРНОЕ",
                    FirstPosting = 1,
                    NumberOfPostings = 10
                };
                TermPosting[] postings = connection.ReadPostings(parameters);
                Assert.AreEqual(8, postings.Length);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }
    }
}
