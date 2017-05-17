// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RetroRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RetroRecord
    {
        #region Properties
        private bool _isNew;

        ///<summary>
        /// 
        ///</summary>
        [MapIgnore]
        [Browsable(false)]
        public bool IsNew
        {
            [DebuggerStepThrough]
            get
            {
                return _isNew;
            }
            [DebuggerStepThrough]
            set
            {
                _isNew = value;
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [SortIndex(0)]
        //[DisplayTitle("Инвентарный номер")]
        [MapField("invnum")]
        public int Inventory { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [SortIndex(1)]
        //[DisplayTitle("Штрих-код")]
        [MapField("barcode")]
        public string Barcode { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [SortIndex(6)]
        [MapField("whn")]
        [ReadOnly(true)]
        //[DisplayTitle("Дата штрих-кодирования")]
        public DateTime Moment { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("taken")]
        [ReadOnly(true)]
        [SortIndex(7)]
        //[DisplayTitle("Взято на обработку")]
        public byte Taken { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("info")]
        [Browsable(false)]
        public string Info
        {
            [DebuggerStepThrough]
            get
            {
                return string.Join
                    (
                        "|",
                        new string[] { Author, Title, Year }
                    );
            }
            [DebuggerStepThrough]
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Author = string.Empty;
                    Title = string.Empty;
                    Year = string.Empty;
                }
                else
                {
                    string[] tokens = value.Split('|');
                    if (tokens.Length > 0)
                    {
                        Author = tokens[0];
                        if (tokens.Length > 1)
                        {
                            Title = tokens[1];
                            if (tokens.Length > 2)
                            {
                                Year = tokens[2];
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [MapIgnore]
        [SortIndex(2)]
        //[DisplayTitle("Автор")]
        public string Author { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapIgnore]
        [SortIndex(3)]
        //[DisplayTitle("Заглавие")]
        public string Title { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapIgnore]
        [SortIndex(4)]
        //[DisplayTitle("Год издания")]
        public string Year { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [ReadOnly(true)]
        [SortIndex(5)]
        [MapField("operator")]
        //[DisplayTitle("Оператор")]
        public int Operator { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the new record.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public static RetroRecord CreateNew()
        {
            RetroRecord result = new RetroRecord
            {
                IsNew = true
            };

            return result;
        }

        #endregion
    }
}
