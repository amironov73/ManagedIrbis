// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RestUtility.cs -- utility routines for REST support in IRBIS
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Nancy;

using Newtonsoft.Json;

using RestSharp;

#endregion

namespace RestfulIrbis
{
    /// <summary>
    /// Utility methods for REST support in IRBIS
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class RestUtility
    {
        #region Public methods

        /// <summary>
        /// Convert the request body to object of given type.
        /// </summary>
        [CLSCompliant(false)]
        public static T ConvertRequestBody<T>
            (
                [NotNull] this Request request
            )
        {
            Code.NotNull(request, nameof(request));

            var body = request.Body;
            body.Seek(0, SeekOrigin.Begin);
            var memory = new MemoryStream((int)body.Length);
            body.CopyTo(memory);
            var bytes = memory.ToArray();
            var json = Encoding.UTF8.GetString(bytes);
            var result = JsonConvert.DeserializeObject<T>(json);
            body.Seek(0, SeekOrigin.Begin);

            return result;
        }

        /// <summary>
        /// Set request body to the specified JSON text.
        /// </summary>
        public static void SetJsonRequestBody
            (
                [NotNull] this IRestRequest request,
                [NotNull] string jsonText
            )
        {
            Code.NotNull(request, nameof(request));
            Code.NotNullNorEmpty(jsonText, nameof(jsonText));

            request.RequestFormat = DataFormat.Json;
            request.AddParameter
                (
                    "application/json; charset=utf-8",
                    jsonText,
                    ParameterType.RequestBody
                );
        }

        #endregion
    }
}

#endif
