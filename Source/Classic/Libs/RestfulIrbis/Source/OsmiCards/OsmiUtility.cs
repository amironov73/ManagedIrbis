// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiUtility.cs -- 
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
using System.Web;
using AM;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Readers;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// 
    /// </summary>
    public static class OsmiUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static JObject FindLabel
            (
                JObject obj,
                string label
            )
        {
            JObject result = (JObject) obj["values"].FirstOrDefault
                (
                    b => b["label"].Value<string>() == label
                );

            if (ReferenceEquals(result, null))
            {
                throw new ApplicationException("Block not found: " + label);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build card for reader.
        /// </summary>
        [NotNull]
        public static JObject BuildCardForReader
            (
                [NotNull] JObject templateObject,
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(templateObject, "templateObject");
            Code.NotNull(reader, "reader");

            string name = reader.FamilyName.ThrowIfNull("name");
            string fio = reader.FullName.ThrowIfNull("fio");
            string ticket = reader.Ticket.ThrowIfNull("ticket");

            JObject result = (JObject) templateObject.DeepClone();

            JObject block = FindLabel(result, "Читатель");
            block["value"] = fio;

            block = FindLabel(result, "Ваш личный кабинет");
            string cabinetUrl = CM.AppSettings["cabinetUrl"];
            block["value"] = string.Format
                (
                    cabinetUrl,
                    UrlEncode(name),
                    UrlEncode(ticket)
                );

            block = FindLabel(result, "Поиск и заказ книг");
            string catalogUrl = CM.AppSettings["catalogUrl"];
            block["value"] = string.Format
                (
                    catalogUrl,
                    UrlEncode(name),
                    UrlEncode(ticket)
                );

            //block = (JObject)result["general"];
            //block["serialNo"] = ticket;

            block = (JObject)result["barcode"];
            block.Property("messageType").Remove();
            block.Property("signatureType").Remove();
            block["message"] = ticket;
            block["signature"] = ticket;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string UrlEncode
            (
                string text
            )
        {
            return HttpUtility.UrlEncode(text, Encoding.UTF8);
        }

        #endregion

        #region Object members

        #endregion
    }
}
