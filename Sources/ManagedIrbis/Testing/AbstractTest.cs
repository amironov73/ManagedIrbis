/* AbstractTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Testing
{
    /// <summary>
    /// Abstract test.
    /// </summary>
    [PublicAPI]
    public abstract class AbstractTest
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [CanBeNull]
        public IrbisConnection Connection { get; set; }

        /// <summary>
        /// Output.
        /// </summary>
        [CanBeNull]
        public AbstractOutput Output { get; set; }

        /// <summary>
        /// Path to test data.
        /// </summary>
        [CanBeNull]
        public string DataPath { get; set; }

        /// <summary>
        /// Test execution context.
        /// </summary>
        [CanBeNull]
        public TestContext Context { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Write some text.
        /// </summary>
        public void Write
            (
                [CanBeNull] string text
            )
        {
            if (ReferenceEquals(Output, null))
            {
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Output.Write(text);
        }

        /// <summary>
        /// Write some formatted text.
        /// </summary>
        public void Write
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            if (ReferenceEquals(Output, null))
            {
                return;
            }

            string text = string.Format(format, args);
            Output.Write(text);
        }

        /// <summary>
        /// Write some object.
        /// </summary>
        public void Write
            (
                [CanBeNull] object obj
            )
        {
            string text = obj.NullableToVisibleString();
            Write(text);
        }

        /// <summary>
        /// Write some error text.
        /// </summary>
        public void WriteError
            (
                [CanBeNull] string text
            )
        {
            if (ReferenceEquals(Output, null))
            {
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Output.WriteError(text);
        }

        /// <summary>
        /// Write some formatted error text.
        /// </summary>
        public void WriteError
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            if (ReferenceEquals(Output, null))
            {
                return;
            }

            string text = string.Format(format,args);
            Output.WriteError(text);
        }

        #endregion
    }
}
