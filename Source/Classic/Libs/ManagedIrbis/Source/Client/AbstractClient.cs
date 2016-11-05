/* AbstractClient.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractClient
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Connected?
        /// </summary>
        public virtual bool Connected { get { return true; } }

        /// <summary>
        /// Current database.
        /// </summary>
        [NotNull]
        public virtual string Database { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Format records.
        /// </summary>
        public virtual string[] FormatRecords
            (
                int[] mfns,
                string format
            )
        {
            return new string[0];
        }

        /// <summary>
        /// Get user server INI-file.
        /// </summary>
        /// <returns></returns>
        public virtual IniFile GetUserIniFile()
        {
            IniFile result = new IniFile();

            return result;
        }

        /// <summary>
        /// Read file.
        /// </summary>
        [CanBeNull]
        public virtual string ReadFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            return null;
        }

        /// <summary>
        /// Read INI-file.
        /// </summary>
        [CanBeNull]
        public virtual IniFile ReadIniFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            IniFile result = null;

            string text = ReadFile(fileSpecification);
            if (!string.IsNullOrEmpty(text))
            {
                result = new IniFile();
                StringReader reader = new StringReader(text);
                result.Read(reader);
            }

            return result;
        }

        /// <summary>
        /// Read MNU file.
        /// </summary>
        [CanBeNull]
        public virtual MenuFile ReadMenuFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            MenuFile result = null;

            string text = ReadFile(fileSpecification);
            if (!string.IsNullOrEmpty(text))
            {
                StringReader reader = new StringReader(text);
                result = MenuFile.ParseStream(reader);
            }

            return result;
        }

        /// <summary>
        /// Read record.
        /// </summary>
        [CanBeNull]
        public virtual MarcRecord ReadRecord
            (
                int mfn
            )
        {
            return null;
        }

        /// <summary>
        /// Search records.
        /// </summary>
        [NotNull]
        public virtual int[] Search
            (
                [CanBeNull] string expression
            )
        {
            return new int[0];
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // Nothing to do here.
        }

        #endregion

        #region Object members

        #endregion
    }
}
