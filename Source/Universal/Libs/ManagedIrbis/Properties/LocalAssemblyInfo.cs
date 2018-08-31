// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalAssemblyInfo.cs -- local information about assembly
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
[assembly: AssemblyTitle("ManagedIrbis")]
[assembly: AssemblyDescription("Managed runtime for IRBIS system")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("ManagedIrbis")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7d0f7e61-2c17-4903-b53d-0f534c5718e1")]

[assembly: AllowPartiallyTrustedCallers]

#if FW4

[assembly: SecurityRules(SecurityRuleSet.Level1)]

#endif
