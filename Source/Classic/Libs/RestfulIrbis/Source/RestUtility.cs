// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RestUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

using Nancy.IO;
#if FW4

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Linq;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Nancy;

using Newtonsoft.Json;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class RestUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert the request body to object of given type.
        /// </summary>
        public static T ConvertRequestBody<T>
            (
                [NotNull] Request request
            )
        {
            Code.NotNull(request, "request");

            RequestStream body = request.Body;
            body.Seek(0, SeekOrigin.Begin);
            MemoryStream memory = new MemoryStream((int)body.Length);
            body.CopyTo(memory);
            byte[] bytes = memory.ToArray();
            string json = Encoding.UTF8.GetString(bytes);
            T result = JsonConvert.DeserializeObject<T>(json);
            body.Seek(0, SeekOrigin.Begin);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
