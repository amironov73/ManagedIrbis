/* IrbisCeptor.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace IrbisCeptor
{
    class Program
    {
        private static int Counter;

        private static int LocalPort;
        private static int RemotePort;
        private static IPAddress RemoteIP;
        private static int BufferSize;
        private static string DataPath;

        public static byte[] ReadToEnd
            (
                Stream stream,
                int portion
            )
        {
            MemoryStream result = new MemoryStream();

            while (true)
            {
                byte[] buffer = new byte[portion];
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                result.Write(buffer, 0, read);
            }

            return result.ToArray();
        }

        static void WriteBuffer
            (
                int counter,
                string prefix,
                string suffix,
                byte[] buffer,
                int length
            )
        {
            string fileName = string.Format
                (
                    "{0}{1:00000000}{2}.packet",
                    prefix,
                    counter,
                    suffix
                );

            using (Stream stream = File.Create(fileName))
            {
                stream.Write(buffer, 0, length);
            }
        }

        static void HandleClient
            (
                TcpClient downClient
            )
        {
            try
            {
                int count = Interlocked.Increment(ref Counter);

                Console.WriteLine("Got downstream: {0}", count);

                NetworkStream downStream = downClient.GetStream();

                IPAddress address = RemoteIP;
                IPEndPoint endPoint = new IPEndPoint(address, RemotePort);
                TcpClient upClient = new TcpClient();
                upClient.Connect(endPoint);
                Console.WriteLine("Connected to upstream");

                byte[] buffer1 = new byte[BufferSize];
                int readed1 = downStream.Read(buffer1, 0, buffer1.Length);
                //byte[] buffer1 = ReadToEnd (downStream, BufferSize);
                //int readed1 = buffer1.Length;
                Console.WriteLine("Readed downstream {0}", readed1);

                NetworkStream upStream = upClient.GetStream();
                upStream.Write(buffer1, 0, readed1);
                Console.WriteLine("Written to upstream");

                Task.Factory.StartNew
                    (
                        () => WriteBuffer
                            (
                                count,
                                DataPath,
                                "dn",
                                buffer1,
                                readed1
                            )
                    );

                //byte[] buffer2 = new byte[BufferSize];
                //int readed2 = upStream.Read(buffer2, 0, buffer2.Length);
                byte[] buffer2 = ReadToEnd (upStream, 100 * 1024);
                int readed2 = buffer2.Length;
                Console.WriteLine("Readed upstream {0}", readed2);

                downStream.Write(buffer2, 0, readed2);
                Console.WriteLine("Written downstream");

                Task.Factory.StartNew
                    (
                        () => WriteBuffer
                            (
                                count, 
                                DataPath,
                                "up",
                                buffer2,
                                readed2
                            )
                    );

                upClient.Close();
                Console.WriteLine("Closed upstream");

                downClient.Close();
                Console.WriteLine("Closed downstream");

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void Main()
        {
            try
            {
                LocalPort = int.Parse(CM.AppSettings["local-port"]);
                RemotePort = int.Parse(CM.AppSettings["remote-port"]);
                RemoteIP = IPAddress.Parse(CM.AppSettings["remote-ip"]);
                BufferSize = int.Parse(CM.AppSettings["buffer-size"]);
                DataPath = CM.AppSettings["data-path"];

                if (!Directory.Exists(DataPath))
                {
                    Directory.CreateDirectory(DataPath);
                }

                string[] files = Directory.GetFiles(DataPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                IPAddress address = IPAddress.Any;
                IPEndPoint endPoint = new IPEndPoint(address, LocalPort);
                TcpListener listener = new TcpListener(endPoint);
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Task.Factory.StartNew
                        (
                            () =>
                            HandleClient(client)
                        );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
