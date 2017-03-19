/* RfidTagList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using OBID.TagHandler;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// RFID tag list.
    /// </summary>
    [PublicAPI]
    public sealed class RfidTagList
    {
        #region Properties

        /// <summary>
        /// Handlers.
        /// </summary>
        [CLSCompliant(false)]
        public FedmIscTagHandler_ISO15693_NXP_ICODE_SLI[] Handlers
        {
            get
            {
                return _dictionary
                    .Values
                    .ToArray();
            }
        }

        /// <summary>
        /// Identifiers.
        /// </summary>
        public string[] Identifiers
        {
            get
            {
                return _dictionary
                    .Keys
                    .ToArray();
            }
        }

        /// <summary>
        /// Index.
        /// </summary>
        [CLSCompliant(false)]
        public FedmIscTagHandler_ISO15693_NXP_ICODE_SLI this[string uid]
        {
            get
            {
                if (string.IsNullOrEmpty(uid))
                {
                    throw new ArgumentNullException("uid");
                }
                if (!_dictionary.ContainsKey("uid"))
                {
                    throw new RfidException("No such UID: " + uid);
                }

                return _dictionary[uid];
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        [CLSCompliant(false)]
        public RfidTagList
        (
            Dictionary<string, FedmIscTagHandler> source
        )
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source");
            }
            
            _dictionary = new Dictionary<string, FedmIscTagHandler_ISO15693_NXP_ICODE_SLI>();
            
            foreach (var pair in source)
            {
                FedmIscTagHandler_ISO15693_NXP_ICODE_SLI handler = pair.Value as FedmIscTagHandler_ISO15693_NXP_ICODE_SLI;
                if (!ReferenceEquals(handler, null))
                {
                    _dictionary.Add
                    (
                        pair.Key,
                        handler
                    );
                }
            }
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, FedmIscTagHandler_ISO15693_NXP_ICODE_SLI> _dictionary;

        #endregion

        #region Public methods

        #endregion
    }
}