/* SuggestAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SuggestAttribute
        : Attribute
    {
        #region Properties

        ///<summary>
        /// 
        ///</summary>
        public Type Type { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SuggestAttribute
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            Type = type;
        }

        #endregion
    }
}
