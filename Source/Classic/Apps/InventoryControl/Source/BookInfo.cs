// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace InventoryControl
{
    [Serializable]
    public sealed class BookInfo
        : IComparable<BookInfo>,
        IEquatable<BookInfo>
    {
        #region Properties

        public int Ordinal { get; set; }

        public int Mfn { get; set; }

        public bool Error { get; set; }

        public bool Control { get; set; }

        public int Amount { get; set; }

        public string Ksu { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public string Year { get; set; }

        public string Price { get; set; }

        public string ErrorText { get; set; }

        public string Sign { get; set; }

        public DateTime CheckedDate { get; set; }

        public string Remark { get; set; }

        public string Barcode { get; set; }

        public string Issue { get; set; }

        #endregion

        #region Public methods

        public void SetError
            (
                string format,
                params object[] args
            )
        {
            Error = true;
            string text = string.Format
                (
                    format,
                    args
                );
            if (!string.IsNullOrEmpty(ErrorText))
            {
                text = ErrorText
                       + Environment.NewLine
                       + text;
            }
            ErrorText = text;
        }

        #endregion

        #region IComparable members

        public int CompareTo(BookInfo other)
        {
            return Mfn - other.Mfn;
        }

        #endregion

        #region IEquatable members

        public bool Equals(BookInfo other)
        {
            // ReSharper disable PossibleNullReferenceException

            return Mfn == other.Mfn;

            // ReSharper restore PossibleNullReferenceException
        }

        #endregion

        #region Object members

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode

            return Mfn;

            // ReSharper restore NonReadonlyMemberInGetHashCode
        }

        public override string ToString()
        {
            return $"{Number}: {Description}";
        }

        #endregion
    }
}
