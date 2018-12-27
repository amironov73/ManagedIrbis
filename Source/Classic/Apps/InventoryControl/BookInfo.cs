/* BookInfo.cs
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

        public bool MultiBook { get; set; }

        public int Amount { get; set; }

        public string Ksu { get; set; }

        public string SaveKsu { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public string SaveDescription { get; set; }

        public string Year { get; set; }

        public string Price { get; set; }

        public string ErrorText { get; set; }

        public decimal ThePrice { get; set; }

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
            return Mfn == other.Mfn;
        }

        #endregion

        #region Object members

        private sealed class MfnEqualityComparer : IEqualityComparer<BookInfo>
        {
            public bool Equals(BookInfo x, BookInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Mfn == y.Mfn;
            }

            public int GetHashCode(BookInfo obj)
            {
                return obj.Mfn;
            }
        }

        private static readonly IEqualityComparer<BookInfo> MfnComparerInstance
            = new MfnEqualityComparer();

        public static IEqualityComparer<BookInfo> MfnComparer
        {
            get { return MfnComparerInstance; }
        }

        public override string ToString()
        {
            return $"{Number}: {Description}";
        }

        #endregion
    }
}
