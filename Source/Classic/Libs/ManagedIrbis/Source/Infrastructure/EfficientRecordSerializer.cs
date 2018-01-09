// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EfficientRecordSerializer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Fast and efficient serializer for <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class EfficientRecordSerializer
    {
        #region Public methods

        /// <summary>
        /// Deserialize the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        public static MarcRecord Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            MarcRecord result = new MarcRecord();
            result.Fields.BeginUpdate();
            int fieldCount = reader.ReadInt16();
            result.Fields.EnsureCapacity(fieldCount);
            for (int fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
            {
                int tag = reader.ReadInt16();
                RecordField field = new RecordField(tag)
                {
                    Value = reader.ReadNullableString()
                };
                result.Fields.Add(field);
                int subfieldCount = reader.ReadInt16();
                for (int subCount = 0; subCount < subfieldCount; subCount++)
                {
                    char code = (char) reader.ReadByte();
                    SubField subField = new SubField(code)
                    {
                        Value = reader.ReadNullableString()
                    };
                    field.SubFields.Add(subField);
                }
            }

            result.EndUpdate();
            return result;
        }

        /// <summary>
        /// Serialize the <see cref="MarcRecord"/>.
        /// </summary>
        public static void Serialize
            (
                [NotNull] BinaryWriter writer,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(record, "record");

            unchecked
            {
                short fieldCount = (short) record.Fields.Count;
                writer.Write(fieldCount);
                for (short fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
                {
                    RecordField field = record.Fields[fieldIndex];
                    short tag = (short) field.Tag;
                    writer.Write(tag);
                    writer.WriteNullable(field.Value);
                    short subfieldCount = (short) field.SubFields.Count;
                    writer.Write(subfieldCount);
                    for (short subIndex = 0; subIndex < subfieldCount; subIndex++)
                    {
                        SubField subField = field.SubFields[subIndex];
                        byte code = (byte) subField.Code;
                        writer.Write(code);
                        writer.WriteNullable(subField.Value);
                    }
                }
            }
        }

        #endregion
    }
}
