// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseInfoLite.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseInfoLite
    {
        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static DatabaseInfoLite[] FromDatabaseInfo
            (
                DatabaseInfo[] source
            )
        {
            DatabaseInfoLite[] result = new DatabaseInfoLite[source.Length];

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

        #endregion

        #region Object members

        #endregion
    }
}
