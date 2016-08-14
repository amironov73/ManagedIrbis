/* CommandLineSettings.cs -- settings for command line
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM.CommandLine
{
    public static class CommandLineSettings
    {
        #region Constants

        /// <summary>
        /// Default argument delimiter.
        /// </summary>
        public const char DefaultArgumentDelimiter = '"';

        /// <summary>
        /// Default prefix for response files.
        /// </summary>
        public const char DefaultResponsePrefix = '@';

        /// <summary>
        /// Default prefix for switches.
        /// </summary>
        public const char DefaultSwitchPrefix = '-';

        /// <summary>
        /// Default separator for values.
        /// </summary>
        public const char DefaultValueSeparator = ':';

        #endregion

        #region Properties

        /// <summary>
        /// Argument delimiter.
        /// </summary>
        public static char ArgumentDelimiter { get; set; }

        /// <summary>
        /// Prefix for response files.
        /// </summary>
        /// <remarks>Default value: <code>'@'</code>.</remarks>
        public static char ResponsePrefix { get; set; }

        /// <summary>
        /// Prefix for switches.
        /// </summary>
        /// <remarks>Default value: <code>'-'</code>.</remarks>
        public static char SwitchPrefix { get; set; }

        /// <summary>
        /// Separator for switch value.
        /// </summary>
        /// <remarks>Default value: <code>':'</code>.</remarks>
        public static char ValueSeparator { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static CommandLineSettings()
        {
            ArgumentDelimiter = DefaultArgumentDelimiter;
            ResponsePrefix = DefaultResponsePrefix;
            SwitchPrefix = DefaultSwitchPrefix;
            ValueSeparator = DefaultValueSeparator;
        }
        #endregion
    }
}
