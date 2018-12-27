/* CeContext.cs
 */

#region Using directives

using System;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using ManagedIrbis.Fields;

#endregion

namespace InventoryControl
{
    public class CeContext
        : DbManager
    {
        #region Properties

        public string DatabaseName { get; private set; }

        /// <summary>
        /// Экземпляры.
        /// </summary>
        public Table<ExemplarInfo> Exemplar
        {
            get { return GetTable<ExemplarInfo>(); }
        }

        #endregion

        #region Construction

        public CeContext
            (
                string databaseName
            )
            : base
            (
                new SqlCeDataProvider(),
                _GetConnectionString(databaseName)
            )
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("databaseName");
            }

            DatabaseName = databaseName;
        }

        #endregion

        #region Private members

        private static string _GetConnectionString
            (
                string databaseName
            )
        {
            return $@"DataSource={databaseName};";
        }

        #endregion

        #region Public methods

        public void TruncateTable(string tableName)
        {
            SetCommand("delete from [" + tableName + "]")
                .ExecuteNonQuery();
        }

        public void TruncateExemplar()
        {
            TruncateTable("exemplars");
        }

        #endregion
    }
}
