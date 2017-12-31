// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XmlUtility.cs -- useful routines for XML manipulations
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using AM.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Xml
{
    /// <summary>
    /// Collection of useful routines for XML manipulations.
    /// </summary>
    public static class XmlUtility
    {
        #region Private members

        private static Dictionary<string, XmlSerializer> _serializers;

        private static void _CreateSerializers()
        {
            lock (typeof(XmlUtility)) //-V3090
            {
                if (ReferenceEquals(_serializers, null))
                {
                    _serializers = new Dictionary<string, XmlSerializer>();
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear cached serializers.
        /// </summary>
        public static void ClearCache()
        {
            if (!ReferenceEquals(_serializers, null))
            {
                lock (_serializers)
                {
                    _serializers.Clear();
                }
            }
        }

        /// <summary>
        /// Deserialize object from file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Object.</returns>
        public static object Deserialize
            (
                [NotNull] string fileName, 
                [NotNull] XmlSerializer serializer
            )
        {
            Code.FileExists(fileName, "fileName");
            Code.NotNull(serializer, "serializer");

            using (Stream stream = File.OpenRead(fileName))
            {
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserialize object from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Deserialize<T>
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            XmlSerializer serializer = new XmlSerializer
                (
                    typeof(T)
                );

            return (T)Deserialize
                (
                    fileName,
                    serializer
                );
        }

        /// <summary>
        /// Deserialize the string.
        /// </summary>
        public static T DeserializeString<T>
            (
                [NotNull] string xml
            )
        {
            Code.NotNullNorEmpty(xml, "xml");

            StringReader reader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }

#if !ANDROID

        /// <summary>
        /// Get serializer for tagged classes.
        /// </summary>
        /// <param name="assembly">Assembly to scan.</param>
        /// <param name="tagName">Tag.</param>
        /// <param name="mainType">Main type.</param>
        /// <returns>Serializer.</returns>
        public static XmlSerializer GetSerializer
            (
                [NotNull] Assembly assembly,
                [NotNull] string tagName,
                [NotNull] Type mainType
            )
        {
            Code.NotNull(assembly, "assembly");
            Code.NotNullNorEmpty(tagName, "tagName");
            Code.NotNull(mainType, "mainType");

            _CreateSerializers();
            lock (_serializers)
            {
                XmlSerializer ser = _serializers[tagName];

                if (ser != null)
                {
                    return ser;
                }

                Type[] xtraTypes = TaggedClassesCollector.Collect
                    (assembly, tagName);

                ser = new XmlSerializer(mainType, xtraTypes);
                _serializers.Add(tagName, ser);

                return ser;
            }
        }

#endif

///// <summary>
///// Get serializer for tagged classes. Scan all assemblies.
///// </summary>
///// <param name="tagName">Tag.</param>
///// <param name="mainType">Main type.</param>
///// <returns>Serializer.</returns>
//public static XmlSerializer GetSerializer
//    (
//        string tagName,
//        Type mainType
//    )
//{
//    Code.NotNullNorEmpty(tagName, "tagName");
//    Code.NotNull(mainType, "mainType");

//    _CreateSerializers();
//    lock (_serializers)
//    {
//        if (_serializers.ContainsKey(tagName))
//        {
//            return _serializers[tagName];
//        }

//        Type[] xtraTypes = TaggedClassesCollector.Collect(tagName);

//        XmlSerializer ser = new XmlSerializer(mainType, xtraTypes);
//        _serializers.Add(tagName, ser);

//        return ser;
//    }
//}

        /// <summary>
        /// Serialize object to file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="obj">Object.</param>
        public static void Serialize
            (
                [NotNull] string fileName,
                [NotNull] XmlSerializer serializer,
                object obj
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(serializer, "serializer");
            Code.NotNull(obj, "obj");

            using (Stream stream = File.Create(fileName))
            {
                serializer.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// Serialize object to file.
        /// </summary>
        public static void Serialize<T>
            (
                [NotNull] string fileName,
                T obj
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            XmlSerializer serializer
                = new XmlSerializer(obj.GetType());
            Serialize(fileName, serializer, obj);
        }

#if !UAP

        /// <summary>
        /// Dumps the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public static XmlReader Dump
            (
                [NotNull] this XmlReader reader
            )
        {
            Console.WriteLine
                (
                    "{0} {1}={2}",
                    reader.NodeType,
                    reader.LocalName,
                    reader.Value
                );

            return reader;
        }
#endif


        /// <summary>
        /// Reads the trimmed string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static string ReadTrimmedString
            (
                [NotNull] this XmlReader reader
            )
        {
            //string result = reader.ReadString();
            string result = reader.ReadElementContentAsString();

            if (result != null)
            {
                result = result.Trim();
            }
            return result;
        }

        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool GetBoolean
            (
                [NotNull] this XmlReader reader,
                [NotNull] string name
            )
        {
            string value = reader.GetAttribute(name);
            return !string.IsNullOrEmpty(value) && bool.Parse(value);
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static int GetInt32
            (
                [NotNull] this XmlReader reader,
                [NotNull] string name
            )
        {
            string value = reader.GetAttribute(name);
            return string.IsNullOrEmpty(value)
                ? 0
                : int.Parse(value);
        }

        /// <summary>
        /// Gets the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T GetEnum<T>
            (
                [NotNull] this XmlReader reader,
                string name
            )
        {
            string value = reader.GetAttribute(name);
            return string.IsNullOrEmpty(value)
                       ? default(T)
                       : (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Serialize to string without standard
        /// XML header and namespaces.
        /// </summary>
        [NotNull]
        public static string SerializeShort
            (
                [NotNull] object obj
            )
        {
            Code.NotNull(obj, "obj");

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NewLineHandling = NewLineHandling.None
            };
            StringBuilder output = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj, namespaces);
            }

            return output.ToString();
        }

        #endregion
    }
}
