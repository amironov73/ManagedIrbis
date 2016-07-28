/* Xml.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE

#region Using directives

using System;

#endregion

namespace System.Xml.Serialization
{
    public class XmlElementAttribute
        : Attribute
    {
        public XmlElementAttribute()
        {
        }

        public XmlElementAttribute(string name)
        {
        }
    }

    public class XmlAttributeAttribute
        : Attribute
    {
        public XmlAttributeAttribute()
        {
        }

        public XmlAttributeAttribute(string name)
        {
        }
    }

    public class XmlRootAttribute
        : Attribute
    {
        public XmlRootAttribute()
        {
        }

        public XmlRootAttribute(string name)
        {
        }
    }

    public class XmlIgnoreAttribute
        : Attribute
    {
    }

    public class XmlTextAttribute
        : Attribute
    {
    }

    public class XmlArrayAttribute
        : Attribute
    {
        public XmlArrayAttribute()
        {
        }

        public XmlArrayAttribute(string name)
        {
        }
    }

    public class XmlArrayItemAttribute
        : Attribute
    {
        public XmlArrayItemAttribute()
        {
        }

        public XmlArrayItemAttribute(string name)
        {
        }
    }
}

#endif
