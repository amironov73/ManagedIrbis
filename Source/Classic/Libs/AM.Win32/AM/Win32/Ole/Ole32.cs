// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Ole32.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	public static class Ole32
	{
		#region Constants

		/// <summary>
		/// Name of the dynamic linking library.
		/// </summary>
		public const string DllName = "Ole32.dll";

		#endregion

		#region Private members
		#endregion

		#region Public methods

		/// <summary>
		/// Creates the bind CTX.
		/// </summary>
		/// <param name="reserved">The reserved.</param>
		/// <param name="ppbc">The PPBC.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern int CreateBindCtx 
			( 
			int reserved, 
			out IBindCtx ppbc 
			);

		/// <summary>
		/// Gets the running object table.
		/// </summary>
		/// <param name="reserved">The reserved.</param>
		/// <param name="prot">The prot.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern int GetRunningObjectTable 
			( 
			int reserved, 
			out IRunningObjectTable prot 
			);


		#endregion
	}
}
