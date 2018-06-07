#if UAP

namespace System
{
    /// <summary>
    /// For compatibility.
    /// </summary>
    public enum PlatformID
    {
        /// <summary>
        /// For compatibility.
        /// </summary>
        Win32S        = 0,

        /// <summary>
        /// For compatibility.
        /// </summary>
        Win32Windows  = 1,

        /// <summary>
        /// For compatibility.
        /// </summary>
        Win32NT       = 2,

        /// <summary>
        /// For compatibility.
        /// </summary>
        WinCE         = 3,

        /// <summary>
        /// For compatibility.
        /// </summary>
        Unix          = 4,

        /// <summary>
        /// For compatibility.
        /// </summary>
        Xbox          = 5,

        /// <summary>
        /// For compatibility.
        /// </summary>
        MacOSX        = 6
    }

    /// <summary>
    /// Operating system.
    /// </summary>
    public sealed class OperatingSystem
    {
        private readonly Version _version;
        private readonly PlatformID _platform;
        private readonly string _servicePack;
        private string _versionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        public OperatingSystem(PlatformID platform, Version version)
            : this(platform, version, null)
        {
        }

        internal OperatingSystem(PlatformID platform, Version version, string servicePack)
        {
            _platform = platform;
            _version = (Version) version;
            _servicePack = servicePack;
        }

        /// <summary>
        /// Platform
        /// </summary>
        public PlatformID Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// Service pack.
        /// </summary>
        public string ServicePack
        {
            get {
                if( _servicePack == null) {
                    return string.Empty;
                }

                return _servicePack;
            }
        }

        /// <summary>
        /// Version.
        /// </summary>
        public Version Version
        {
            get { return _version; }
        }

        /// <inheritdoc cref="object.ToString" />
        public override String ToString()
        {
            return VersionString;
        }

        /// <summary>
        /// Version string.
        /// </summary>
        public String VersionString
        {
            get {
                if(_versionString != null)
                {
                    return _versionString;
                }

                String os;
                switch(_platform)
                {
                    case PlatformID.Win32NT:
                        os = "Microsoft Windows NT ";
                        break;

                    case PlatformID.Win32Windows:
                        if (_version.Major > 4 ||
                            _version.Major == 4 && _version.Minor > 0)
                            os = "Microsoft Windows 98 ";
                        else
                            os = "Microsoft Windows 95 ";
                        break;

                    case PlatformID.Win32S:
                        os = "Microsoft Win32S ";
                        break;

                    case PlatformID.WinCE:
                        os = "Microsoft Windows CE ";
                        break;

                    case PlatformID.MacOSX:
                        os = "Mac OS X ";
                        break;

                    default:
                        os = "<unknown> ";
                        break;
                }

                if( String.IsNullOrEmpty(_servicePack))
                {
                    _versionString = os + _version;
                }
                else
                {
                    _versionString = os + _version.ToString(3) + " " + _servicePack;
                }

                return _versionString;
            }
        }
    }
}

#endif
