// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationManager.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Menus;
using ManagedIrbis.Readers;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReservationManager
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReservationManager
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get reader name.
        /// </summary>
        [CanBeNull]
        public string GetReaderName
            (
                [NotNull] string ticket
            )
        {
            Code.NotNullNorEmpty(ticket, "ticket");

            string previousDatabase = Provider.Database;
            Provider.Database = ReaderUtility.DatabaseName;
            try
            {
                string expression = string.Format
                (
                    "\"{0}{1}\"",
                    ReaderUtility.IdentifierPrefix,
                    ticket
                );
                int[] found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    return null;
                }
                if (found.Length != 1)
                {
                    return null;
                }
                int mfn = found[0];
                MarcRecord record = Provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    return null;
                }
                ReaderInfo reader = ReaderInfo.Parse(record);
                string result = reader.FullName;

                return result;
            }
            finally
            {
                Provider.Database = previousDatabase;
            }
        }

        /// <summary>
        /// Get the <see cref="MarcRecord"/> for the resource.
        /// </summary>
        [NotNull]
        public MarcRecord GetResourceRecord
            (
                [NotNull] ReservationInfo resource
            )
        {
            Code.NotNull(resource, "resource");

            MarcRecord result = GetResourceRecord
                (
                    resource.Room.ThrowIfNull("resource.Room"),
                    resource.Number.ThrowIfNull("resource.Number")
                );
            if (ReferenceEquals(result, null))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        /// Get record for the resource.
        /// </summary>
        [CanBeNull]
        public MarcRecord GetResourceRecord
                (
                    [NotNull] string room,
                    [NotNull] string number
                )
        {
            Code.NotNullNorEmpty(room, "room");
            Code.NotNullNorEmpty(number, "number");

            string expression = string.Format
                (
                    "\"{0}{1}\" * \"{2}{3}\"",
                    ReservationUtility.RoomPrefix,
                    room,
                    ReservationUtility.NumberPrefix,
                    number
                );
            int[] found = Provider.Search(expression);
            if (found.Length != 1)
            {
                return null;
            }
            MarcRecord result = Provider.ReadRecord(found[0]);

            return result;
        }

        /// <summary>
        /// Assign the resource to the reader.
        /// </summary>
        public ReservationInfo AssignResource
            (
                [NotNull] ReservationInfo resource,
                [NotNull] string ticket
            )
        {
            Code.NotNull(resource, "resource");
            Code.NotNullNorEmpty(ticket, "ticket");

            MarcRecord record = GetResourceRecord(resource);
            resource = ReservationInfo.ParseRecord(record);
            if (!resource.Status.OneOf
                (
                    ReservationStatus.Free,
                    ReservationStatus.Reserved
                ))
            {
                // TODO some reaction?

                return resource;
            }
            resource.Status = ReservationStatus.Busy;

            string readerName = GetReaderName(ticket);
            HistoryInfo entry = new HistoryInfo
            {
                BeginDate = DateTime.Now,
                Ticket = ticket,
                Name = readerName
            };
            resource.History.Add(entry);

            resource.ApplyToRecord(record);
            Provider.WriteRecord(record);

            return resource;
        }

        /// <summary>
        /// List rooms.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public MenuEntry[] ListRooms()
        {
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    Provider.Database,
                    ReservationUtility.RoomMenu
                );
            MenuFile menuFile = Provider.ReadMenuFile(specification);
            if (ReferenceEquals(menuFile, null))
            {
                return new MenuEntry[0];
            }

            return menuFile.Entries.ToArray();
        }

        /// <summary>
        /// List resources.
        /// </summary>
        [NotNull]
        public ReservationInfo[] ListResources
            (
                [CanBeNull] string roomCode
            )
        {
            string expression = string.Format
                (
                    "\"{0}{1}\"",
                    ReservationUtility.RoomPrefix,
                    roomCode
                );
            int[] found = Provider.Search(expression);
            List<ReservationInfo> result
                = new List<ReservationInfo>(found.Length);
            foreach (int mfn in found)
            {
                MarcRecord record = Provider.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    ReservationInfo item = ReservationInfo.ParseRecord(record);
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Release the resource.
        /// </summary>
        [NotNull]
        public ReservationInfo ReleaseResource
            (
                [NotNull] ReservationInfo resource
            )
        {
            Code.NotNull(resource, "resource");

            MarcRecord record = GetResourceRecord(resource);
            resource = ReservationInfo.ParseRecord(record);
            resource.Status = ReservationStatus.Free;
            HistoryInfo entry = resource.History.LastOrDefault();
            if (!ReferenceEquals(entry, null))
            {
                entry.EndDate = DateTime.Now;
            }

            resource.ApplyToRecord(record);
            Provider.WriteRecord(record);

            return resource;
        }

        /// <summary>
        /// Create claim.
        /// </summary>
        [NotNull]
        public ReservationInfo CreateClaim
            (
                [NotNull] ReservationInfo resource,
                DateTime beginDate,
                TimeSpan duration
            )
        {
            Code.NotNull(resource, "resource");

            MarcRecord record = GetResourceRecord(resource);
            resource = ReservationInfo.ParseRecord(record);
            if (resource.Status.OneOf(ReservationStatus.Free, ReservationStatus.Busy))
            {
                throw new IrbisException();
            }
            resource.Status = ReservationStatus.Reserved;
            ReservationClaim claim = new ReservationClaim
            {
                BeginDateTime = beginDate,
                Duration = duration
            };
            resource.Claims.Add(claim);
            resource.ApplyToRecord(record);
            Provider.WriteRecord(record);

            return resource;
        }

        #endregion

        #region Object members

        #endregion
    }
}
