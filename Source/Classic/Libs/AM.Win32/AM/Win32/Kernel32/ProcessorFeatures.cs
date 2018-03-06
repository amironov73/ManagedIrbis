// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProcessorFeatures.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Processor features.
	/// </summary>
	public enum ProcessorFeatures
	{
		/// <summary>
		/// Pentium:  In rare circumstances, a floating-point 
		/// precision error can occur.
		/// </summary>
		PF_FLOATING_POINT_PRECISION_ERRATA,

		/// <summary>
		/// <para>Floating-point operations are emulated using a 
		/// software emulator.</para>
		/// <para>This function returns a nonzero value if 
		/// floating-point operations are emulated; otherwise, 
		/// it returns zero.</para>
		/// <para>Windows NT 4.0: This function returns zero if 
		/// floating-point operations are emulated; otherwise, 
		/// it returns a nonzero value. This behavior is a bug that 
		/// is fixed in later versions.</para>
		/// </summary>
		PF_FLOATING_POINT_EMULATED,

		/// <summary>
		/// The compare and exchange double operation is available 
		/// (Pentium, MIPS, and Alpha).
		/// </summary>
		PF_COMPARE_EXCHANGE_DOUBLE,

		/// <summary>
		/// The MMX instruction set is available.
		/// </summary>
		PF_MMX_INSTRUCTIONS_AVAILABLE,

		/// <summary>
		/// ???
		/// </summary>
		PF_PPC_MOVEMEM_64BIT_OK,

		/// <summary>
		/// ???
		/// </summary>
		PF_ALPHA_BYTE_INSTRUCTIONS,

		/// <summary>
		/// The SSE instruction set is available.
		/// </summary>
		PF_XMMI_INSTRUCTIONS_AVAILABLE,

		/// <summary>
		/// The 3D-Now instruction set is available.
		/// </summary>
		PF_3DNOW_INSTRUCTIONS_AVAILABLE,

		/// <summary>
		/// The RDTSC instruction is available.
		/// </summary>
		PF_RDTSC_INSTRUCTION_AVAILABLE,

		/// <summary>
		/// The processor is PAE-enabled.
		/// </summary>
		PF_PAE_ENABLED,

		/// <summary>
		/// The SSE2 instruction set is available.
		/// </summary>
		PF_XMMI64_INSTRUCTIONS_AVAILABLE
	}
}
