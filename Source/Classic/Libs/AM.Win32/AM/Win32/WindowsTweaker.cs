// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WindowsTweaker.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	public static class WindowsTweaker
	{
		#region Public methods

		/// <summary>
		/// Sets the legal notice at logon time.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="noticeText">The message text.</param>
		public static void SetLegalNotice
			(
				string caption,
				string noticeText
			)
		{
			using (RegistryKey key = Registry.LocalMachine.OpenSubKey
				(
					@"Software\Microsoft\WindowsNT\CurrentVersion\Winlogon",
					true
				))
			{
				key.SetValue 
					( 
						"LegalNoticeCaption",
						caption,
						RegistryValueKind.String
					);
				key.SetValue 
					( 
						"LegalNoticeText",
						noticeText,
						RegistryValueKind.String
					);
			}
		}

		#endregion
	}
}
