/* BLToolkit.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE

#region Using directives

using System;

#endregion

namespace BLToolkit.DataAccess
{
    public class BLToolkitDummyClass
    {
    }
}

namespace BLToolkit.Mapping
{
    public class TableNameAttribute
        : Attribute
    {
        public TableNameAttribute(string name)
        {
        }
    }

    public class MapIgnoreAttribute
        : Attribute
    {
    }

    public class MapFieldAttribute
        : Attribute
    {
        public MapFieldAttribute()
        {
        }

        public MapFieldAttribute(string name)
        {
        }
    }
}

#endif
