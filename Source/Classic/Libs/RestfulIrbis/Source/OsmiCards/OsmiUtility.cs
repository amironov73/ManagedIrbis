// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* OsmiUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using System;
using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Readers;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

#if !ANDROID && !UAP && !NETCORE

using System.Web;

using CM=System.Configuration.ConfigurationManager;

#endif

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

            //if (ReferenceEquals(result, null))
            //{
            //    throw new Exception("Block not found: " + label);
            //}

            return result;
        }

        #endregion

        #region Public methods

#if !ANDROID && !UAP && !NETCORE

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
            if (!ReferenceEquals(block, null))
            {
                block["value"] = fio;
            }

            block = FindLabel(result, "Ваш личный кабинет");
            if (!ReferenceEquals(block, null))
            {
                string cabinetUrl = CM.AppSettings["cabinetUrl"];
                block["value"] = string.Format
                    (
                        cabinetUrl,
                        UrlEncode(name),
                        UrlEncode(ticket)
                    );
            }

            block = FindLabel(result, "Поиск и заказ книг");
            if (!ReferenceEquals(block, null))
            {
                string catalogUrl = CM.AppSettings["catalogUrl"];
                block["value"] = string.Format
                    (
                        catalogUrl,
                        UrlEncode(name),
                        UrlEncode(ticket)
                    );
            }

            block = (JObject)result["barcode"];
            if (!ReferenceEquals(block, null))
            {
                block.Property("messageType")?.Remove();
                block.Property("signatureType")?.Remove();
                block["message"] = ticket;
                block["signature"] = ticket;
            }

            return result;
        }

        /// <summary>
        /// Убираем '-empty-'.
        /// </summary>
        [CanBeNull]
        public static string NullForEmpty
            (
                [CanBeNull] this string value
            )
        {
            return value.SameString("-empty-")
                ? null
                : value;
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

#endif

        #endregion

        #region Object members

        #endregion
    }
}

#endif
