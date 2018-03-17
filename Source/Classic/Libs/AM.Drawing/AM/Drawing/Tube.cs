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

using Newtonsoft.Json;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// <see cref="Palette"/> item.
    /// </summary>
    [PublicAPI]
    [XmlRoot("tube")]
    public sealed class Tube
        : IDisposable,
        IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Name of the tube.
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        private Color _color;

        /// <summary>
        /// Color of the tube.
        /// </summary>
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
        /// Get the brush.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush Brush => _brush ?? (_brush = new SolidBrush(Color));

        private Pen _pen;

        /// <summary>
        /// Get the pen.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Pen Pen => _pen ?? (_pen = new Pen(Color));

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
        /// Constructor.
        /// </summary>
        public Tube
            (
                [NotNull] string name,
                Color color
            )
        {
            Code.NotNull(name, "name");

            Name = name;
            Color = color;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs an implicit conversion from
        /// <see cref="Tube"/> to <see cref="System.Drawing.Brush"/>.
        /// </summary>
        public static implicit operator Brush([NotNull] Tube tube)
        {
            return tube.Brush;
        }

        /// <summary>
        /// Performs an implicit conversion from
        /// <see cref="Tube"/> to <see cref="System.Drawing.Pen"/>.
        /// </summary>
        public static implicit operator Pen([NotNull] Tube tube)
        {
            return tube.Pen;
        }

        /// <summary>
        /// Performs an implicit conversion from
        /// <see cref="Tube"/> to <see cref="System.Drawing.Color"/>.
        /// </summary>
        public static implicit operator Color([NotNull] Tube tube)
        {
            return tube.Color;
        }

        #endregion

        #region IXmlSerializable members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("name");
            _color = ColorTranslator.FromHtml(reader.GetAttribute("color"));
            reader.Read();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString
                (
                    "color",
                    ColorTranslator.ToHtml(Color)
                );
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!ReferenceEquals(_brush, null))
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

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(Tube other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Tube)) return false;

            return Equals((Tube)obj);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode

            return Name?.GetHashCode() ?? 0;
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"{Color} [{Name}]";
        }

        #endregion
    }
}
