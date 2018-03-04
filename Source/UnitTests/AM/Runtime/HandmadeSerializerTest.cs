using System;
using System.IO;
using System.Linq;

using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

namespace UnitTests.AM.Runtime
{
    class MyFirstClass
        : IHandmadeSerializable
    {
        public int FirstField { get; set; }

        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FirstField = reader.ReadPackedInt32();
        }

        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(FirstField);
        }
    }

    class MySecondClass
        : IHandmadeSerializable
    {
        public string SecondField { get; set; }

        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            SecondField = reader.ReadNullableString();
        }

        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(SecondField);
        }
    }

    class MyThirdClass
        : IHandmadeSerializable
    {
        public bool ThirdField { get; set; }

        public double FourthField { get; set; }

        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThirdField = reader.ReadBoolean();
            FourthField = reader.ReadDouble();
        }

        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(ThirdField);
            writer.Write(FourthField);
        }
    }

    [TestClass]
    public class HandmadeSerializerTest
    {
        [TestMethod]
        public void HandmadeSerializer_Roundtrip_1()
        {
            MyFirstClass first1 = new MyFirstClass
            {
                FirstField = 73
            };
            MySecondClass second1 = new MySecondClass
            {
                SecondField = "Hello"
            };
            MyThirdClass third1 = new MyThirdClass
            {
                ThirdField = true,
                FourthField = 123.45
            };

            HandmadeSerializer serializer = new HandmadeSerializer();
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                serializer.Serialize(writer, first1);
                serializer.Serialize(writer, second1);
                serializer.Serialize(writer, third1);
            }

            byte[] buffer = stream.ToArray();

            stream = new MemoryStream(buffer);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                MyFirstClass first2 = (MyFirstClass)serializer.Deserialize(reader);
                Assert.AreEqual(first1.FirstField, first2.FirstField);

                MySecondClass second2 = (MySecondClass)serializer.Deserialize(reader);
                Assert.AreEqual(second1.SecondField, second2.SecondField);

                MyThirdClass third2 = (MyThirdClass)serializer.Deserialize(reader);
                Assert.AreEqual(third1.ThirdField, third2.ThirdField);
                Assert.AreEqual(third1.FourthField, third2.FourthField);
            }
        }

        [TestMethod]
        public void HandmadeSerializer_Roundtrip_2()
        {
            MyFirstClass first1 = new MyFirstClass
            {
                FirstField = 73
            };
            MySecondClass second1 = new MySecondClass
            {
                SecondField = "Hello"
            };
            MyThirdClass third1 = new MyThirdClass
            {
                ThirdField = true,
                FourthField = 123.45
            };

            HandmadeSerializer serializer = new HandmadeSerializer(PrefixLength.Short)
            {
                Namespace = "UnitTests.AM.Runtime",
                Assembly = typeof(MyFirstClass).Assembly
            };
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                serializer.Serialize(writer, first1);
                serializer.Serialize(writer, second1);
                serializer.Serialize(writer, third1);
            }

            byte[] buffer = stream.ToArray();

            stream = new MemoryStream(buffer);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                MyFirstClass first2 = (MyFirstClass)serializer.Deserialize(reader);
                Assert.AreEqual(first1.FirstField, first2.FirstField);

                MySecondClass second2 = (MySecondClass)serializer.Deserialize(reader);
                Assert.AreEqual(second1.SecondField, second2.SecondField);

                MyThirdClass third2 = (MyThirdClass)serializer.Deserialize(reader);
                Assert.AreEqual(third1.ThirdField, third2.ThirdField);
                Assert.AreEqual(third1.FourthField, third2.FourthField);
            }
        }

        [TestMethod]
        public void HandmadeSerializer_Roundtrip_3()
        {
            MyFirstClass first1 = new MyFirstClass
            {
                FirstField = 73
            };
            MySecondClass second1 = new MySecondClass
            {
                SecondField = "Hello"
            };
            MyThirdClass third1 = new MyThirdClass
            {
                ThirdField = true,
                FourthField = 123.45
            };

            HandmadeSerializer serializer = new HandmadeSerializer(PrefixLength.Moderate)
            {
                Assembly = typeof(MyFirstClass).Assembly
            };
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                serializer.Serialize(writer, first1);
                serializer.Serialize(writer, second1);
                serializer.Serialize(writer, third1);
            }

            byte[] buffer = stream.ToArray();

            stream = new MemoryStream(buffer);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                MyFirstClass first2 = (MyFirstClass)serializer.Deserialize(reader);
                Assert.AreEqual(first1.FirstField, first2.FirstField);

                MySecondClass second2 = (MySecondClass)serializer.Deserialize(reader);
                Assert.AreEqual(second1.SecondField, second2.SecondField);

                MyThirdClass third2 = (MyThirdClass)serializer.Deserialize(reader);
                Assert.AreEqual(third1.ThirdField, third2.ThirdField);
                Assert.AreEqual(third1.FourthField, third2.FourthField);
            }
        }

        [TestMethod]
        public void HandmadeSerializer_Array_1()
        {
            MyFirstClass[] sourceArray =
            {
                new MyFirstClass{FirstField = 1},
                new MyFirstClass{FirstField = 2},
                new MyFirstClass{FirstField = 3},
                new MyFirstClass{FirstField = 4},
            };

            HandmadeSerializer serializer = new HandmadeSerializer();
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // ReSharper disable once CoVariantArrayConversion
                serializer.Serialize(writer, sourceArray);
            }

            byte[] buffer = stream.ToArray();

            stream = new MemoryStream(buffer);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                IHandmadeSerializable[] tempArray = serializer.DeserializeArray(reader);
                MyFirstClass[] targetArray = tempArray.Cast<MyFirstClass>().ToArray();
                Assert.AreEqual(sourceArray.Length, targetArray.Length);

                for (int i = 0; i < sourceArray.Length; i++)
                {
                    Assert.AreEqual
                        (
                            sourceArray[i].FirstField,
                            targetArray[i].FirstField
                        );
                }
            }
        }

        [TestMethod]
        public void HandmadeSerializer_Array_2()
        {
            MyFirstClass[] sourceArray =
            {
                new MyFirstClass{FirstField = 1},
                new MyFirstClass{FirstField = 2},
                new MyFirstClass{FirstField = 3},
                new MyFirstClass{FirstField = 4},
            };

            HandmadeSerializer serializer = new HandmadeSerializer(PrefixLength.Short)
            {
                Namespace = "UnitTests.AM.Runtime",
                Assembly = typeof(MyFirstClass).Assembly
            };
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // ReSharper disable once CoVariantArrayConversion
                serializer.Serialize(writer, sourceArray);
            }

            byte[] buffer = stream.ToArray();

            stream = new MemoryStream(buffer);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                IHandmadeSerializable[] tempArray = serializer.DeserializeArray(reader);
                MyFirstClass[] targetArray = tempArray.Cast<MyFirstClass>().ToArray();
                Assert.AreEqual(sourceArray.Length, targetArray.Length);

                for (int i = 0; i < sourceArray.Length; i++)
                {
                    Assert.AreEqual
                        (
                            sourceArray[i].FirstField,
                            targetArray[i].FirstField
                        );
                }
            }
        }

        [TestMethod]
        public void HandmadeSerializer_Array_3()
        {
            MyFirstClass[] sourceArray =
            {
                new MyFirstClass{FirstField = 1},
                new MyFirstClass{FirstField = 2},
                new MyFirstClass{FirstField = 3},
                new MyFirstClass{FirstField = 4},
            };

            HandmadeSerializer serializer = new HandmadeSerializer(PrefixLength.Moderate)
            {
                Assembly = typeof(MyFirstClass).Assembly
            };
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // ReSharper disable once CoVariantArrayConversion
                serializer.Serialize(writer, sourceArray);
            }

            byte[] buffer = stream.ToArray();

            stream = new MemoryStream(buffer);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                IHandmadeSerializable[] tempArray = serializer.DeserializeArray(reader);
                MyFirstClass[] targetArray = tempArray.Cast<MyFirstClass>().ToArray();
                Assert.AreEqual(sourceArray.Length, targetArray.Length);

                for (int i = 0; i < sourceArray.Length; i++)
                {
                    Assert.AreEqual
                        (
                            sourceArray[i].FirstField,
                            targetArray[i].FirstField
                        );
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HandmadeSerializer_Serialize_1()
        {
            MyFirstClass first1 = new MyFirstClass
            {
                FirstField = 73
            };

            HandmadeSerializer serializer = new HandmadeSerializer((PrefixLength)44);
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                serializer.Serialize(writer, first1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandmadeSerializer_Serialize_2()
        {
            MyFirstClass[] array = new MyFirstClass[0];

            HandmadeSerializer serializer = new HandmadeSerializer();
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // ReSharper disable once CoVariantArrayConversion
                serializer.Serialize(writer, array);
            }
        }
    }
}
