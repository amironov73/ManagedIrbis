// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AssemblyInfo.cs -- general information about assembly
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("AM.Core")]
[assembly: AssemblyDescription("ArsMagna core services")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("AM.Core")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f9562edd-2489-486e-8992-c0bdfc26fa61")]

[assembly: AllowPartiallyTrustedCallers]

#if FW4

[assembly: SecurityRules(SecurityRuleSet.Level1)]

#endif
