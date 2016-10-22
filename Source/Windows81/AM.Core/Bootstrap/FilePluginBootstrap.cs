#if WIN81

using System;

using MvvmCross.Platform.Plugins;

namespace AM.Bootstrap
{
    /// <summary>
    /// Bootstrap.
    /// </summary>
    [CLSCompliant(false)]
    public class FilePluginBootstrap
        : MvxPluginBootstrapAction<MvvmCross.Plugins.File.PluginLoader>
    {
    }
}

#endif

