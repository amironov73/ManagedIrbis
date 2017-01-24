// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ControlCenter.cs -- 
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

using AM.Windows.Forms;

using ManagedIrbis;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace EasyBinder
{
    public static class ControlCenter
    {
        #region Properties

        public static IrbisConnection Connection { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static void Connect()
        {
            string connectionString = CM.AppSettings["connectionString"];
            Connection = new IrbisConnection(connectionString);
        }

        public static void Disconnect()
        {
            if (!ReferenceEquals(Connection, null))
            {
                Connection.Dispose();
                Connection = null;
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
