// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tube.cs -- palette item
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
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
    [XmlRoot("tube")]
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public sealed class Tube
        : //Component,
        IDisposable,
        IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        private Color _color;

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        [XmlAttribute("color")]
        public Color Color
        {
            get { return _color; }
            set
            {
                Dispose();
                _color = value;
            }
        }

        private Brush _brush;

        /// <summary>
        /// Gets the brush.
        /// </summary>
        /// <value>The brush.</value>
        [XmlIgnore]
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public Brush Brush
        {
            get { return _brush ?? (_brush = new SolidBrush(Color)); }
        }

        private Pen _pen;

        /// <summary>
        /// Gets the pen.
        /// </summary>
        /// <value>The pen.</value>
        [XmlIgnore]
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public Pen Pen
        {
            get { return _pen ?? (_pen = new Pen(Color)); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Tube"/> class.
        /// </summary>
        public Tube()
        {
            Name = "No name";
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Tube"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public Tube(Color color)
        {
            Name = "No name";
            Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Tube"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Tube(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Tube"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="name">The name.</param>
        public Tube(Color color, string name)
        {
            Color = color;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Tube"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        public Tube(string name, Color color)
        {
            Color = color;
            Name = name;
        }

        /// <summary>
        /// Releases unmanaged resources and performs 
        /// other cleanup operations before the
        /// <see cref="Tube"/> is reclaimed by garbage collection.
        /// </summary>
        ~Tube()
        {
            Dispose();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs an implicit conversion from 
        /// <see cref="Tube"/> to <see cref="System.Drawing.Brush"/>.
        /// </summary>
        /// <param name="tube">The tube.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Brush ( Tube tube )
        {
            return tube.Brush;
        }

        /// <summary>
        /// Performs an implicit conversion from 
        /// <see cref="Tube"/> to <see cref="System.Drawing.Pen"/>.
        /// </summary>
        /// <param name="tube">The tube.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Pen ( Tube tube )
        {
            return tube.Pen;
        }

        /// <summary>
        /// Performs an implicit conversion from 
        /// <see cref="Tube"/> to <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="tube">The tube.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color ( Tube tube )
        {
            return tube.Color;
        }

        #endregion

        #region IXmlSerializable members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The 
        /// <see cref="T:System.Xml.XmlReader"/> stream from 
        /// which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("name");
            _color = ColorTranslator
                .FromHtml(reader
                    .GetAttribute("color"));
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The 
        /// <see cref="T:System.Xml.XmlWriter"/> stream 
        /// to which the object is serialized.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name",Name);
            writer.WriteAttributeString
                (
                    "color",
                    ColorTranslator.ToHtml(Color)
                );
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
            if (_brush != null)
            {
                _brush.Dispose();
                _brush = null;
            }
            if (_pen != null)
            {
                _pen.Dispose();
                _pen = null;
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// 
        /// </summary>
        public bool Equals(Tube other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        /// <summary>
        /// Determines whether the specified 
        /// <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> 
        /// to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified 
        /// <see cref="System.Object"/> is equal to this instance; 
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Tube)) return false;
            return Equals((Tube) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use 
        /// in hashing algorithms and data structures like 
        /// a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that 
        /// represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents 
        /// this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}]", Color, Name);
        }

        #endregion
    }
}
