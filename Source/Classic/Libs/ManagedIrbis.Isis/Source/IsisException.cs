// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Isis
{
    /// <summary>
    /// Exception occured during ISIS32.DLL interop.
    /// </summary>
    [Serializable]
    public class IsisException
        : IrbisException
    {
        #region Properties

        /// <summary>
        /// Error code returned from ISIS32.DLL.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// MFN of erroneous record.
        /// </summary>
        public int Mfn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsisException
            (
                int code
            )
            : base(_FormatMessage(code, -1))
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsisException
            (
                int code,
                int mfn
            )
            : base(_FormatMessage(code, mfn))
        {
            Code = code;
            Mfn = mfn;
        }

        #endregion

        #region Private members

        private static string _FormatMessage
            (
                int code,
                int mfn
            )
        {
            return string.Format
                (
                    "ISIS code = {0} [{1}] ({2}) (MFN={3})",
                    code,
                    Enum.Parse
                        (
                            typeof(IsisErrorCodes),
                            code.ToString()
                        ),
                    Environment.NewLine,
                    mfn
                );
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ReferenceEquals"/>
        public override string ToString()
        {
            return _FormatMessage(Code, Mfn);
        }

        #endregion
    }
}
