/* DumbSerializer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Dumb serializer.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class DumbSerializer
    {
        #region Public methods

        public static void SaveToStream
            (
                IList<int> list,
                BinaryWriter writer
            )
        {
            int count = list.Count;
            writer.WritePackedInt32(count);
            for (int i = 0; i < count; i++)
            {
                writer.WritePackedInt32(list[i]);
            }
        }

        public static void SaveToStream
            (
                IList<long> list,
                BinaryWriter writer
            )
        {
            int count = list.Count;
            writer.WritePackedInt32(count);
            for (int i = 0; i < count; i++)
            {
                writer.WritePackedInt64(list[i]);
            }
        }

        public static void SaveToStream
            (
                IList<short> list,
                BinaryWriter writer
            )
        {
            int count = list.Count;
            writer.WritePackedInt32(count);
            for (int i = 0; i < count; i++)
            {
                writer.Write(list[i]);
            }
        }

        public static void SaveToStream
            (
                IList<string> list,
                BinaryWriter writer
            )
        {
            int count = list.Count;
            writer.WritePackedInt32(count);
            for (int i = 0; i < count; i++)
            {
                writer.Write(list[i]);
            }
        }

        public static void SaveToStream
            (
                IList<double> list,
                BinaryWriter writer
            )
        {
            int count = list.Count;
            writer.WritePackedInt32(count);
            for (int i = 0; i < count; i++)
            {
                writer.Write(list[i]);
            }
        }

        public static void SaveToStream
            (
                IList<float> list,
                BinaryWriter writer
            )
        {
            int count = list.Count;
            writer.WritePackedInt32(count);
            for (int i = 0; i < count; i++)
            {
                writer.Write(list[i]);
            }
        }

        public static void SaveListToStream<T>
            (
                [NotNull] IList<T> list,
                [NotNull] BinaryWriter writer
            )
        {
            if (list is IList<int>)
                SaveToStream((IList<int>)list, writer);
            else if (list is IList<long>)
                SaveToStream((IList<long>)list, writer);
            else if (list is IList<short>)
                SaveToStream((IList<short>)list, writer);
            else if (list is IList<string>)
                SaveToStream((IList<string>)list, writer);
            else if (list is IList<double>)
                SaveToStream((IList<double>)list, writer);
            else if (list is IList<float>)
                SaveToStream((IList<float>)list, writer);
        }

        #endregion
    }
}
