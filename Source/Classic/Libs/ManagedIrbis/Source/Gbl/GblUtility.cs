// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblUtility.cs -- utility routines for GBL handling
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
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Gbl.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Utility routines for GBL file handling.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class GblUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Encode statements for IRBIS64 protocol.
        /// </summary>
        [NotNull]
        public static string EncodeStatements
            (
                [NotNull] GblStatement[] statements
            )
        {
            Code.NotNull(statements, "statements");

            StringBuilder result = new StringBuilder();
            result.Append("!0");
            result.Append(GblStatement.Delimiter);

            foreach (GblStatement item in statements)
            {
                result.Append(item.EncodeForProtocol());
            }
            result.Append(GblStatement.Delimiter);

            return result.ToString();
        }

        /// <summary>
        /// Restore <see cref="GblFile"/> from JSON.
        /// </summary>
        [NotNull]
        public static GblFile FromJson
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            Log.Error
                (
                    "GblUtility::FromJson: "
                    + "not implemented"
                );

            throw new NotImplementedException();

#else

            GblFile result = JsonConvert
                .DeserializeObject<GblFile>(text);

            return result;

#endif
        }

        /// <summary>
        /// Restore <see cref="GblFile"/> from JSON.
        /// </summary>
        [NotNull]
        public static GblFile FromXml
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            XmlSerializer serializer = new XmlSerializer(typeof(GblFile));
            using (StringReader reader = new StringReader(text))
            {
                GblFile result = (GblFile) serializer.Deserialize(reader);

                return result;
            }
        }

        //=================================================

        /// <summary>
        /// Build text representation of <see cref="GblNode"/>'s.
        /// </summary>
        public static void NodesToText
            (
                [NotNull] StringBuilder builder,
                [NotNull] IEnumerable<GblNode> nodes
            )
        {
            Code.NotNull(builder, "builder");
            Code.NotNull(nodes, "nodes");

            bool first = true;
            foreach (GblNode node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(' ');
                }
                builder.Append(node);
                first = false;
            }
        }

        //=================================================

        /// <summary>
        /// Parses the local JSON file.
        /// </summary>
        [NotNull]
        public static GblFile ParseLocalJsonFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if WINMOBILE || PocketPC

            Log.Error
                (
                    "GblUtility::ParseLocalJsonFile: "
                    + "not implemented"
                );

            throw new NotImplementedException ();

#else

            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            GblFile result = FromJson(text);

            return result;
#endif
        }

        /// <summary>
        /// Parses the local XML file.
        /// </summary>
        [NotNull]
        public static GblFile ParseLocalXmlFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if WINMOBILE || PocketPC

            Log.Error
                (
                    "GblUtility::ParseLocalXmlFile: "
                    + "not implemented"
                );

            throw new NotImplementedException();

#else

            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            GblFile result = FromXml(text);

            return result;

#endif
        }


        /// <summary>
        /// Saves <see cref="GblFile"/> to local JSON file.
        /// </summary>
        public static void SaveLocalJsonFile
            (
                [NotNull] this GblFile gbl,
                [NotNull] string fileName
            )
        {
            Code.NotNull(gbl, "gbl");
            Code.NotNullNorEmpty(fileName, "fileName");

#if WINMOBILE || PocketPC

            Log.Error
                (
                    "GblUtility::SaveLocalJsonFile: "
                    + "not implemented"
                );

            throw new NotImplementedException();

#else

            string contents = JArray.FromObject(gbl)
                .ToString(Formatting.Indented);

            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );

#endif
        }

        /// <summary>
        /// Convert <see cref="GblFile"/> to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this GblFile gbl
            )
        {
            Code.NotNull(gbl, "gbl");

#if WINMOBILE || PocketPC

            Log.Error
                (
                    "GblUtility::ToJson: "
                    + "not implemented"
                );

            throw new NotImplementedException();

#else

            string result = JObject.FromObject(gbl)
                .ToString(Formatting.None);

            return result;

#endif
        }

        /// <summary>
        /// Converts the <see cref="GblFile"/> to XML.
        /// </summary>
        [NotNull]
        public static string ToXml
            (
                [NotNull] this GblFile gbl
            )
        {
            Code.NotNull(gbl, "gbl");

            XmlSerializer serializer
                = new XmlSerializer(typeof(GblFile));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, gbl);

            return writer.ToString();
        }

        #endregion
    }
}
