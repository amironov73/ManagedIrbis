using System;
using System.IO;

using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class StreamPackerTest
    {
        static void _Check
            (
                uint first, 
                uint count
            )
        {
            using (Stream stream = new MemoryStream((int)(count * 4)))
            {
                uint i, j, k;

                for (i = 0, k = first; i < count; i++, k++)
                {
                    StreamPacker.PackUInt32(stream, k);
                }

                stream.Position = 0;

                for (i = 0, k = first; i < count; i++, k++)
                {
                    j = StreamPacker.UnpackUInt32(stream);
                    if (j != k)
                        throw new Exception
                            (
                                string.Format("failed on {0}: {1}", k, j)
                            );
                }
            }
        }

        // TODO solve QTAgent32.exe problem

        //[TestMethod]
        //public void TestPackUInt32()
        //{
        //    uint last = 10000000;
        //    uint block = 10;
        //    for (uint first = 0; first < last; first += block)
        //    {
        //        //Console.Write("\r{0:0000000000} of {1:0000000000}", first, last);
        //        _Check(first, block);
        //    }
        //}
    }
}
