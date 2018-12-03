// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalAssemblyInfo.cs
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
[assembly: AssemblyTitle("UnsafeIrbis")]
[assembly: AssemblyDescription("Unsafe managed runtime for IRBIS system")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("UnsafeIrbis")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8bafaddc-4de1-4902-b901-93daf68742d8")]

[assembly: AllowPartiallyTrustedCallers]

[assembly: SecurityRules(SecurityRuleSet.Level1)]
