/* UITest.cs -- UI test description
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Xml.Serialization;

using AM;
using AM.Json;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    /// <summary>
    /// UI test description.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("test")]
    // ReSharper disable once InconsistentNaming
    public sealed class UITest
    {
        #region Properties

        /// <summary>
        /// Class name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("class")]
        [JsonProperty("class")]
        public string ClassName { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion

        #region Public methods

        [NotNull]
        public static UITest[] LoadFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            UITest[] result
                = JsonUtility.ReadObjectFromFile<UITest[]>
                (
                    fileName
                );

            return result;
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public void Run
            (
                [CanBeNull] IWin32Window ownerWindow
            )
        {
            Type type = Type.GetType
                (
                    ClassName.ThrowIfNull("ClassName"),
                    true
                );
            IUITest testObject = (IUITest) Activator.CreateInstance
                (
                    type
                );
            testObject.Run(ownerWindow);
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
