// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XmlNamespaces.cs -- some well known XML namespaces
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Xml
{
    /// <summary>
    /// Some well known XML namespaces.
    /// </summary>
    [PublicAPI]
    public static class XmlNamespaces
    {
        #region Constants

        /// <summary>
        /// Msbuild.exe
        /// </summary>
        public const string Msbuild =
            "http://schemas.microsoft.com/developer/msbuild/2003";

        /// <summary>
        /// Msdata.
        /// </summary>
        public const string Msdata = "urn:schemas-microsoft-com:xml-msdata";

        /// <summary>
        /// MS XSL.
        /// </summary>
        public const string Msxsl = "urn:schemas-microsoft-com:xslt";

        /// <summary>
        /// SOAP.
        /// </summary>
        public const string Soap = "http://www.w3.org/2003/05/soap-envelope";

        /// <summary>
        /// WSDL.
        /// </summary>
        public const string Wsdl = "http://schemas.xmlsoap.org/wsdl";

        /// <summary>
        /// WSH.
        /// </summary>
        public const string Wsh = "http://schemas.microsoft.com/WindowsScriptHost";

        /// <summary>
        /// XHTML.
        /// </summary>
        public const string Xhtml = "http://www.w3.org/1999/xhtml";

        /// <summary>
        /// XLINK.
        /// </summary>
        public const string Xlink = "http://www.w3.org/1999/xlink";

        /// <summary>
        /// XSI.
        /// </summary>
        public const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// XSD.
        /// </summary>
        public const string Xsd = "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// XSL.
        /// </summary>
        public const string Xsl = "http://www.w3.org/1999/XSL/Transform";

        #endregion
    }
}