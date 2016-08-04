/* UpdateUserListTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class UpdateUserListTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [TestMethod]
        public void TestUpdateUserList()
        {
            IrbisConnection connection = Connection.ThrowIfNull();

            UserInfo[] oldUsers = connection.ListUsers();
            UserInfo tylerDurden = new UserInfo
            {
                Name = "TylerDurden",
                Password = "FightClub" + DateTime.Now.Ticks,
                Cataloger = "tyler.ini"
            };

            List<UserInfo> newUsers = oldUsers
                .Where(user => user.Name != "TylerDurden")
                .ToList();
            newUsers.Add(tylerDurden);

            connection.UpdateUserList(newUsers.ToArray());

            Write
                (
                    string.Join
                    (
                        ", ",
                        oldUsers.Select(u => u.Name).ToArray()
                    )
                );

            Write
                (
                    " | was: {0}, now: {1}",
                    oldUsers.Length,
                    newUsers.Count
                );
        }

        #endregion
    }
}
