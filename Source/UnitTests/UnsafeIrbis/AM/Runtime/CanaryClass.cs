using System;
using System.IO;

using UnsafeAM;
using UnsafeAM.IO;
using UnsafeAM.Runtime;

using JetBrains.Annotations;

namespace UnitTests.UnsafeAM.Runtime
{
    public sealed class CanaryClass
        : IHandmadeSerializable,
          IDisposable
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public void RestoreFromStream(BinaryReader reader)
        {
            Name = reader.ReadNullableString();
            Age = reader.ReadPackedInt32();
        }

        public void SaveToStream(BinaryWriter writer)
        {
            writer.WriteNullable(Name);
            writer.WritePackedInt32(Age);
        }

        public void EnsureSame([NotNull]CanaryClass other)
        {
            if (string.CompareOrdinal(Name, other.Name) != 0
                || Age != other.Age)
            {
                throw new VerificationException();
            }
        }

        public void Dispose()
        {
            // Nothing to do here
        }
    }
}
