/* ComponentModel.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE

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
