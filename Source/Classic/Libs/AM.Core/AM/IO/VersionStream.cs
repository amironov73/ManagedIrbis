// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VersionStream.cs -- versioning stream
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Versioning <see cref="Stream"/>.
    /// </summary>
    /// <seealso cref="NotifyStream"/>
    [PublicAPI]
    [MoonSharpUserData]
    public class VersionStream
        : NotifyStream
    {
        #region Properties

        private int _version;

        /// <summary>
        /// Number of stream content modifications.
        /// </summary>
        public virtual int Version
        {
            [DebuggerStepThrough]
            get
            {
                return _version;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="innerStream"></param>
        public VersionStream(Stream innerStream)
            : base(innerStream)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set version count to zero.
        /// </summary>
        public virtual void ResetVersion()
        {
            _version = 0;
        }

        #endregion

        #region NotifyStream members

        /// <inheritdoc cref="NotifyStream.OnStreamChanged" />
        protected override void OnStreamChanged()
        {
            _version++;
            base.OnStreamChanged();
        }

        #endregion
    }
}
