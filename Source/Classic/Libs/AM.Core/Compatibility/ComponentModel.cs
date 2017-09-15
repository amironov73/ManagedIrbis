// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComponentModel.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if PORTABLE || UAP || WINMOBILE || SILVERLIGHT || WIN81

#region Using directives

using System;

#endregion

namespace System.ComponentModel
{

#if !SILVERLIGHT

    /// <summary>
    /// 
    /// </summary>
    public class BrowsableAttribute
        : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public BrowsableAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public BrowsableAttribute(bool flag)
        {
        }
    }

#endif

#if !WINMOBILE

    /// <summary>
    /// 
    /// </summary>
    public class DesignerCategoryAttribute
        : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DesignerCategoryAttribute(string name)
        {
        }
    }

#endif

    /// <summary>
    /// 
    /// </summary>
    public class DesignerSerializationVisibilityAttribute
        : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibility"></param>
        public DesignerSerializationVisibilityAttribute
            (
                DesignerSerializationVisibility visibility
            )
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DesignerSerializationVisibility
    {
        /// <summary>
        /// 
        /// </summary>
        Hidden = 0
    }

#if !SILVERLIGHT

    /// <summary>
    /// 
    /// </summary>
    public class DescriptionAttribute
        : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public DescriptionAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        public DescriptionAttribute(string description)
        {
        }
    }

#endif

    /// <summary>
    /// 
    /// </summary>
    public class DisplayNameAttribute
        : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public DisplayNameAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DisplayNameAttribute(string name)
        {
        }
    }
}

#endif
