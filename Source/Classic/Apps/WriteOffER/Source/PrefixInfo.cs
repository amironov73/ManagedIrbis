// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PrefixInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace WriteOffER
{
    /// <summary>
    ///
    /// </summary>
    public class PrefixInfo
    {
        /// <summary>
        ///
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Description;
        }
    }
}
