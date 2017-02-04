// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseInfoLite.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System.Diagnostics;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// Substitute for <see cref="DatabaseInfo"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Name} {Description}")]
#endif
    public sealed class DatabaseInfoLite
    {
        #region Properties

        /// <summary>
        /// Name of the database.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description for the database.
        /// </summary>
        [JsonProperty("name")]
        public string Description { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert from <see cref="DatabaseInfo"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static DatabaseInfoLite[] FromDatabaseInfo
            (
                [NotNull][ItemNotNull] DatabaseInfo[] source
            )
        {
            Code.NotNull(source, "source");

            DatabaseInfoLite[] result
                = new DatabaseInfoLite[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[i] = new DatabaseInfoLite
                {
                    Name = source[i].Name,
                    Description = source[i].Description
                };
            }

            return result;
        }

        /// <summary>
        /// Convert from <see cref="DatabaseInfo"/>
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static DatabaseInfo[] ToDatabaseInfo
            (
                [NotNull][ItemNotNull] DatabaseInfoLite[] source
            )
        {
            Code.NotNull(source, "source");

            DatabaseInfo[] result
                = new DatabaseInfo[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[i] = new DatabaseInfo
                {
                    Name = source[i].Name,
                    Description = source[i].Description
                };
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Name.ToVisibleString();
            }

            return string.Format
                (
                    "{0} - {1}",
                    Name,
                    Description
                );
        }

        #endregion
    }
}

#endif
