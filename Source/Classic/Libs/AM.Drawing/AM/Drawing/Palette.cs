// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Palette.cs -- palette of colors
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [XmlRoot("palette")]
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class Palette
        : Collection<Tube>,
        IXmlSerializable,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Tube this[string name]
        {
            get { return _dictionary[name]; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Palette"/> class.
        /// </summary>
        public Palette()
        {
            _dictionary = new Dictionary<string, Tube>();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Palette"/> is reclaimed by garbage collection.
        /// </summary>
        ~Palette()
        {
            Dispose();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, Tube> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public Tube Add(string name, Color color)
        {
            Tube result = new Tube(name, color);
            Add(result);
            return result;
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        protected override void ClearItems()
        {
            Dispose();
            _dictionary.Clear();
            base.ClearItems();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializeFromAttributes()
        {
            PropertyInfo[] properties = GetType()
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes
                    (
                        typeof(PaletteColorAttribute),
                        true
                    );
                foreach (PaletteColorAttribute attribute in attributes)
                {
                    string name = property.Name;
                    Color color = Color.FromName(attribute.Color);
                    Add(name, color);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tube GetTubeFromReflection ()
        {
            StackFrame frame = new StackFrame(1,false);
            //string funcName = _functionRegex
            //    .Match(frame.GetMethod().Name)
            //    .Groups[1].Value;
            string funcName = frame.GetMethod().Name.Substring(4);
            return this[funcName];
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void InsertItem(int index, Tube item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (_dictionary.ContainsKey(item.Name))
            {
                _dictionary.Remove(item.Name);
                //Remove(item.Name);
            }
            _dictionary.Add(item.Name,item);
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void RemoveItem(int index)
        {
            Tube item = this[index];
            _dictionary.Remove(item.Name);
            item.Dispose();
            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void SetItem(int index, Tube item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (_dictionary.ContainsKey(item.Name))
            {
                _dictionary.Remove(item.Name);
                //Remove(item.Name);
            }
            _dictionary.Add(item.Name,item);
            base.SetItem(index, item);
        }

        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Palette));
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Reads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Palette Read(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Palette));
            using (StreamReader reader = new StreamReader(fileName))
            {
                Palette result = (Palette)serializer.Deserialize(reader);
                return result;
            }
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            int index;
            for (index = 0; index < Count; index++)
            {
                if (this[index].Name == name)
                {
                    break;
                }
            }
            if (index >= Count)
            {
                throw new KeyNotFoundException();
            }
            RemoveItem(index);
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated 
        /// with freeing, releasing, or resetting unmanaged 
        /// resources.
        /// </summary>
        public void Dispose()
        {
            foreach (Tube tube in Items)
            {
                tube.Dispose();
            }
        }

        #endregion

        #region IXmlSerializable members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.Read();
            while (true)
            {
                if (reader.Name != "tube")
                {
                    break;
                }
                Tube tube = new Tube();
                ((IXmlSerializable)tube).ReadXml(reader);
                Add(tube);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (Tube tube in Items)
            {
                writer.WriteStartElement("tube");
                ((IXmlSerializable)tube).WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (Tube item in Items)
            {
                result.AppendFormat
                    (
                        "{0}{1}",
                        item,
                        Environment.NewLine
                    );
            }

            return result.ToString();
        }

        #endregion
    }
}
