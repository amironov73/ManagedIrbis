/* BLToolkit.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace BLToolkit
{
    /// <summary>
    /// For compatibility only.
    /// </summary>
    public class DummyClass {}
}

namespace BLToolkit.DataAccess
{
    /// <summary>
    /// For compatibility only.
    /// </summary>
    public class BLToolkitDummyClass
    {
    }
}

namespace BLToolkit.Mapping
{
    /// <summary>
    /// For compatibility only.
    /// </summary>
    public class TableNameAttribute
        : Attribute
    {
        /// <summary>
        /// For compatibility only.
        /// </summary>
        public TableNameAttribute(string name)
        {
        }
    }

    /// <summary>
    /// For compatibility only.
    /// </summary>
    public class MapIgnoreAttribute
        : Attribute
    {
    }

    /// <summary>
    /// For compatibility only.
    /// </summary>
    public class MapFieldAttribute
        : Attribute
    {
        /// <summary>
        /// For compatibility only.
        /// </summary>
        public MapFieldAttribute()
        {
        }

        /// <summary>
        /// For compatibility only.
        /// </summary>
        public MapFieldAttribute(string name)
        {
        }
    }
}

