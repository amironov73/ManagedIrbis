// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FlushIntention.cs -- specifies when to flush the queue of graphics operations
   Ars Magna project, http://arsmagna.ru */

#region Using directives



#endregion

namespace AM.Win32
{
	/// <summary>
	/// The FlushIntention enumeration specifies when 
	/// to flush the queue of graphics operations.
	/// </summary>
	public enum FlushIntention
	{
		/// <summary>
		/// When passed to the Graphics::Flush method, 
		/// specifies that pending rendering operations 
		/// are executed as soon as possible. The Flush 
		/// method is not synchronized with the completion 
		/// of the rendering operations and might return 
		/// before the rendering operations are completed. 
		/// </summary>
		FlushIntentionFlush = 0,

		/// <summary>
		/// When passed to the Graphics::Flush method, 
		/// specifies that pending rendering operations are executed 
		/// as soon as possible. The Flush method is synchronized 
		/// with the completion of the rendering operations; that is, 
		/// it will not return until after the rendering operations 
		/// are completed.
		/// </summary>
		FlushIntentionSync = 1
	}
}