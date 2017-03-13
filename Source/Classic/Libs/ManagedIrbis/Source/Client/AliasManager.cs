// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AliasManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Aliases for databases/servers.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !PocketPC
    [DebuggerDisplay("Count={_aliases.Count}")]
#endif
    public sealed class AliasManager
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AliasManager()
        {
            _aliases = new List<Alias>();
        }

        #endregion

        #region Private members

        private readonly List<Alias> _aliases;

        private Alias _GetAlias
            (
                string name
            )
        {
            foreach (Alias theAlias in _aliases)
            {
                if (theAlias.Name.SameString(name))
                {
                    return theAlias;
                }
            }

            return null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the table.
        /// </summary>
        [NotNull]
        public AliasManager Clear()
        {
            _aliases.Clear();
            return this;
        }

#if !WIN81

        /// <summary>
        /// Read file and create <see cref="AliasManager"/>.
        /// </summary>
        [NotNull]
        public static AliasManager FromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamReader reader
                = new StreamReader
                    (
                        File.OpenRead(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                AliasManager result = new AliasManager();

                while (true)
                {
                    string line1 = reader.ReadLine();
                    string line2 = reader.ReadLine();

                    if (string.IsNullOrEmpty(line1)
                        || string.IsNullOrEmpty(line2))
                    {
                        break;
                    }
                    Alias theAlias = new Alias
                        {
                            Name = line1,
                            Value = line2
                        };
                    result._aliases.Add(theAlias);
                }

                return result;
            }
        }

#endif

        /// <summary>
        /// Get alias value if exists.
        /// </summary>
        [CanBeNull]
        public string Get
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Alias theAlias = _GetAlias(name);
            string result = ReferenceEquals(theAlias, null)
                ? null
                : theAlias.Value;

            return result;
        }

        /// <summary>
        /// List aliases.
        /// </summary>
        [NotNull]
        public string[] ListAliases()
        {
            string[] result = _aliases
                .Select(alias => alias.Name)
                .ToArray();

            return result;
        }

#if !WIN81

        /// <summary>
        /// Save aliases to file.
        /// </summary>
        public void SaveToFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamWriter writer = new StreamWriter
                    (
                        File.Create(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                foreach (Alias theAlias in _aliases)
                {
                    writer.WriteLine(theAlias.Name);
                    writer.WriteLine(theAlias.Value);
                }
            }

        }

#endif

        /// <summary>
        /// Add new or modify existing alias.
        /// </summary>
        [NotNull]
        public AliasManager Set
            (
                [NotNull] string name,
                [CanBeNull] string value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Alias theAlias = _GetAlias(name);
            if (ReferenceEquals(theAlias, null))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    theAlias = new Alias
                    {
                        Name = name,
                        Value = value
                    };
                    _aliases.Add(theAlias);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    _aliases.Remove(theAlias);
                }
                else
                {
                    theAlias.Value = value;
                }
            }

            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
