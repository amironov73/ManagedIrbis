// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComponentModel.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NOTDEF

#region Using directives

using System;

#endregion

namespace System.ComponentModel
{
    public class BrowsableAttribute
        : Attribute
    {
        public BrowsableAttribute()
        {
        }

        public BrowsableAttribute(bool flag)
        {
        }
    }

    public class DesignerCategoryAttribute
        : Attribute
    {
        public DesignerCategoryAttribute(string name)
        {
        }
    }

    public class DesignerSerializationVisibilityAttribute
        : Attribute
    {
        public DesignerSerializationVisibilityAttribute
            (
                DesignerSerializationVisibility visibility
            )
        {

        }
    }

    public enum DesignerSerializationVisibility
    {
        Hidden = 0
    }
}

#endif
