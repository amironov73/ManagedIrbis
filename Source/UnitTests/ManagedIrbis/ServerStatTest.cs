using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ServerStatTest
    {
        [TestMethod]
        public void ServerStat_Construction_1()
        {
            ServerStat stat = new ServerStat();
            Assert.IsNull(stat.RunningClients);
            Assert.AreEqual(0, stat.ClientCount);
            Assert.AreEqual(0, stat.TotalCommandCount);
            Assert.AreEqual(0, stat.Unknown);
        }

        [TestMethod]
        public void ServerStat_Parse_1()
        {
            ResponseBuilder builder = new ResponseBuilder();

            builder.AppendAnsi("10093797\r\n3\r\n9\r\n*\r\n127.0.0.1\r\n" +
              "5555\r\nСервер ИРБИС\r\n*****\r\n*****\r\n" +
              "13.10.2017 22:50:40\r\n*****\r\n*****\r\n*****\r\n1\r\n" +
              "127.0.0.1\r\n5555\r\nandreevama\r\n424042\r\n" +
              "\"Каталогизатор\"\r\n08.11.2017 7:47:05\r\n" +
              "08.11.2017 14:57:32\r\nIRBIS_PREV_TRM\r\n140\r\n2\r\n" +
              "127.0.0.1\r\n5555\r\nАлексееваТВ\r\n714689\r\n" +
              "\"Каталогизатор\"\r\n08.11.2017 7:53:44\r\n" +
              "08.11.2017 15:07:18\r\nIRBIS_SVR_FORMAT\r\n1890\r\n3\r\n" +
              "127.0.0.1\r\n5555\r\nАлексееваТВ\r\n631372\r\n" +
              "\"Каталогизатор\"\r\n08.11.2017 7:54:06\r\n" +
              "08.11.2017 14:55:35\r\nIRBIS_NOOP\r\n169\r\n");

            IrbisConnection connection = new IrbisConnection();
            byte[] query = new byte[0];
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    query,
                    true
                );

            ServerStat stat = ServerStat.Parse(response);
            Assert.AreEqual(10093797, stat.TotalCommandCount);
            Assert.AreEqual(3, stat.ClientCount);
            Assert.AreEqual(9, stat.Unknown);
            Assert.IsNotNull(stat.RunningClients);
            Assert.AreEqual(4, stat.RunningClients.Length);
            Assert.AreEqual("Сервер ИРБИС", stat.RunningClients[0].Name);
            Assert.AreEqual("andreevama", stat.RunningClients[1].Name);
            Assert.AreEqual("\"Каталогизатор\"", stat.RunningClients[1].Workstation);
            Assert.AreEqual("IRBIS_PREV_TRM", stat.RunningClients[1].LastCommand);
        }
    }
}
