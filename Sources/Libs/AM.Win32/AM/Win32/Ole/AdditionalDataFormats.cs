/* AdditionalDataFormats.cs -- additional data formats
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System.Windows.Forms;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Additional OLE data formats.
	/// </summary>
	/// <seealso cref="DataFormats"/>
	public static class AdditionalDataFormats
	{
		#region Constants

		/// <summary>
		/// Descriptor of file group.
		/// </summary>
		/// <seealso cref="FILEGROUPDESCRIPTORA"/>
		/// <seealso cref="FILEGROUPDESCRIPTORW"/>
		/// <seealso cref="FILEDESCRIPTORA"/>
		/// <seealso cref="FILEDESCRIPTORW"/>
		public const string FileGroupDescriptor
			= "FileGroupDescriptor";

		/// <summary>
		/// Descriptor of file group.
		/// </summary>
		/// <seealso cref="FILEGROUPDESCRIPTORA"/>
		/// <seealso cref="FILEGROUPDESCRIPTORW"/>
		/// <seealso cref="FILEDESCRIPTORA"/>
		/// <seealso cref="FILEDESCRIPTORW"/>
		public const string FileGroupDescriptorW
			= "FileGroupDescriptorW";

		/// <summary>
		/// Uniform Resource Locator (URL).
		/// </summary>
		public const string UniformResourceLocator 
			= "UniformResourceLocator";

		/// <summary>
		/// Uniform Resource Locator (URL).
		/// </summary>
		public const string UniformResourceLocatorW 
			= "UniformResourceLocatorW";

		#endregion
	}
}
