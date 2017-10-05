// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UslugaRecord.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    public class UslugaRecord
    {
        #region Properties

        public string Title { get; set; }

        public int Price { get; set; }

        public string Unit { get; set; }

        #endregion

        #region Public methods

        public static UslugaRecord[] ReadFile
        (
            string fileName
        )
        {
            List<UslugaRecord> result = new List<UslugaRecord>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    string[] parts = line.Split(';');
                    if (parts.Length != 3)
                    {
                        continue;
                    }
                    UslugaRecord usluga = new UslugaRecord
                    {
                        Title = parts[0],
                        Price = int.Parse(parts[1]),
                        Unit = parts[2]
                    };
                    result.Add(usluga);
                }
            }

            return result.ToArray();
        }

        #endregion
    }
}
