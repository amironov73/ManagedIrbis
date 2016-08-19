/* GdiPlus.cs -- some missing features from gdiplus.dll
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Some missing features from gdiplus.dll
	/// </summary>
	public static class GdiPlus
	{
		#region Constants

		/// <summary>
		/// Name of the dynamic library file.
		/// </summary>
		public const string DllName = "gdiplus.dll";

		#endregion

		#region Private members

		#endregion

		#region Public methods

		#region Image APIs

		/// <summary>
		/// the load image from stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipLoadImageFromStream
			(
			IntPtr stream,
			out IntPtr image
			);

		/// <summary>
		/// the load image from file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipLoadImageFromFile
			(
			string filename,
			out IntPtr image
			);

		/// <summary>
		/// the load image from stream ICM.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipLoadImageFromStreamICM
			(
			IStream stream,
			out IntPtr image
			);

		/// <summary>
		/// the load image from file ICM.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipLoadImageFromFileICM
			(
			string filename,
			out IntPtr image
			);

		/// <summary>
		/// the clone image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="cloneImage">The clone image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCloneImage
			(
			IntPtr image,
			out IntPtr cloneImage
			);

		/// <summary>
		/// the dispose image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipDisposeImage
			(
			IntPtr image
			);

		/// <summary>
		/// the save image to file.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="clsidEncoder">The CLSID encoder.</param>
		/// <param name="encoderParams">The encoder params.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSaveImageToFile
			(
			IntPtr image,
			string filename,
			Guid clsidEncoder,
			EncoderParameters encoderParams
			);

		/// <summary>
		/// the save image to stream.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="clsidEncoder">The CLSID encoder.</param>
		/// <param name="encoderParams">The encoder params.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSaveImageToStream
			(
			IntPtr image,
			IStream stream,
			Guid clsidEncoder,
			EncoderParameters encoderParams
			);

		/// <summary>
		/// the save add.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="encoderParams">The encoder params.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSaveAdd
			(
			IntPtr image,
			EncoderParameters encoderParams
			);

		/// <summary>
		/// the save add image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="newImage">The new image.</param>
		/// <param name="encoderParams">The encoder params.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSaveAddImage
			(
			IntPtr image,
			IntPtr newImage,
			EncoderParameters encoderParams
			);

		/// <summary>
		/// the get image graphics context.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageGraphicsContext
			(
			IntPtr image,
			out IntPtr graphics
			);

		/// <summary>
		/// the get image bounds.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="srcRect">The SRC rect.</param>
		/// <param name="srcUnit">The SRC unit.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageBounds
			(
			IntPtr image,
			out Rectangle srcRect,
			out GraphicsUnit srcUnit
			);

		/// <summary>
		/// the get image dimension.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageDimension
			(
			IntPtr image,
			out float width,
			out float height
			);

		/// <summary>
		/// the type of the get image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageType
			(
			IntPtr image,
			out ImageType type
			);

		/// <summary>
		/// the width of the get image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageWidth
			(
			IntPtr image,
			out int width
			);

		/// <summary>
		/// the height of the get image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageHeight
			(
			IntPtr image,
			out int height
			);

		/// <summary>
		/// the get image horizontal resolution.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="resolution">The resolution.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageHorizontalResolution
			(
			IntPtr image,
			out float resolution
			);

		/// <summary>
		/// the get image vertical resolution.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="resolution">The resolution.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageVerticalResolution
			(
			IntPtr image,
			out float resolution
			);

		/// <summary>
		/// the get image flags.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="flags">The flags.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageFlags
			(
			IntPtr image,
			out int flags
			);

		/// <summary>
		/// the get image raw format.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageRawFormat
			(
			IntPtr image,
			out Guid format
			);

		/// <summary>
		/// the get image pixel format.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImagePixelFormat
			(
			IntPtr image,
			out PixelFormat format
			);

		/// <summary>
		/// the get image thumbnail.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="thumbWidth">Width of the thumb.</param>
		/// <param name="thumbHeight">Height of the thumb.</param>
		/// <param name="thumbImage">The thumb image.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="callbackData">The callback data.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImageThumbnail
			(
			IntPtr image,
			int thumbWidth,
			int thumbHeight,
			out IntPtr thumbImage,
			Image.GetThumbnailImageAbort callback,
			IntPtr callbackData
			);

		/// <summary>
		/// the size of the get encoder parameter list.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="clsidEncoder">The CLSID encoder.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetEncoderParameterListSize
			(
			IntPtr image,
			ref Guid clsidEncoder,
			out int size
			);

		/// <summary>
		/// the get encoder parameter list.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="clsidEncoder">The CLSID encoder.</param>
		/// <param name="size">The size.</param>
		/// <param name="buffer">The buffer.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetEncoderParameterList
			(
			IntPtr image,
			ref Guid clsidEncoder,
			int size,
			out EncoderParameters buffer
			);

		/// <summary>
		/// the image get frame dimensions count.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipImageGetFrameDimensionsCount
			(
			IntPtr image,
			out int count
			);

		/// <summary>
		/// the image get frame dimensions list.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="dimensionIDs">The dimension I ds.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipImageGetFrameDimensionsList
			(
			IntPtr image,
			Guid dimensionIDs,
			int count
			);

		/// <summary>
		/// the image get frame count.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="dimensionID">The dimension ID.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipImageGetFrameCount
			(
			IntPtr image,
			Guid dimensionID,
			out int count
			);

		/// <summary>
		/// the image select active frame.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="dimensionID">The dimension ID.</param>
		/// <param name="frameIndex">Index of the frame.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipImageSelectActiveFrame
			(
			IntPtr image,
			Guid dimensionID,
			int frameIndex
			);

		/// <summary>
		/// the image rotate flip.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="rfType">Type of the rf.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipImageRotateFlip
			(
			IntPtr image,
			RotateFlipType rfType
			);

		/// <summary>
		/// the get image palette.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="palette">The palette.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImagePalette
			(
			IntPtr image,
			ColorPalette palette,
			int size
			);

		/// <summary>
		/// the set image palette.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetImagePalette
			(
			IntPtr image,
			ColorPalette palette
			);

		/// <summary>
		/// the size of the get image palette.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetImagePaletteSize
			(
			IntPtr image,
			out int size
			);

		/// <summary>
		/// the get property count.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="numOfProperty">The num of property.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetPropertyCount
			(
			IntPtr image,
			out int numOfProperty
			);

		/// <summary>
		/// the get property id list.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="numOfProperty">The num of property.</param>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetPropertyIdList
			(
			IntPtr image,
			int numOfProperty,
			out IntPtr list
			);

		/// <summary>
		/// the size of the get property item.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="propId">The prop id.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetPropertyItemSize
			(
			IntPtr image,
			IntPtr propId,
			out int size
			);

		/// <summary>
		/// the get property item.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="propId">The prop id.</param>
		/// <param name="propSize">Size of the prop.</param>
		/// <param name="buffer">The buffer.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetPropertyItem
			(
			IntPtr image,
			IntPtr propId,
			int propSize,
			IntPtr buffer
			);

		/// <summary>
		/// the size of the get property.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="totalBufferSize">Total size of the buffer.</param>
		/// <param name="numProperties">The num properties.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetPropertySize
			(
			IntPtr image,
			out int totalBufferSize,
			out int numProperties
			);

		/// <summary>
		/// the get all property items.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="totalBufferSize">Total size of the buffer.</param>
		/// <param name="numProperties">The num properties.</param>
		/// <param name="allItems">All items.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetAllPropertyItems
			(
			IntPtr image,
			int totalBufferSize,
			int numProperties,
			IntPtr allItems
			);

		/// <summary>
		/// the remove property item.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="propId">The prop id.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipRemovePropertyItem
			(
			IntPtr image,
			IntPtr propId
			);

		/// <summary>
		/// the set property item.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetPropertyItem
			(
			IntPtr image,
			IntPtr item
			);

		/// <summary>
		/// the image force validation.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipImageForceValidation
			(
			IntPtr image
			);

		#endregion

		#region Bitmap APIs

		/// <summary>
		/// Gdips the create bitmap from stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromStream
			(
			IStream stream,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromFile
			(
			string filename,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from stream ICM.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromStreamICM
			(
			IStream stream,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from file ICM.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromFileICM
			(
			string filename,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from scan0.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="stride">The stride.</param>
		/// <param name="format">The format.</param>
		/// <param name="scan0">The scan0.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromScan0
			(
			int width,
			int height,
			int stride,
			PixelFormat format,
			IntPtr scan0,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from graphics.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="target">The target.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromGraphics
			(
			int width,
			int height,
			IntPtr target,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from direct draw surface.
		/// </summary>
		/// <param name="surface">The surface.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromDirectDrawSurface
			(
			IntPtr surface,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from GDI dib.
		/// </summary>
		/// <param name="gdiBitmapInfo">The GDI bitmap info.</param>
		/// <param name="gdiBitmapData">The GDI bitmap data.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromGdiDib
			(
			ref BITMAPINFO gdiBitmapInfo,
			IntPtr gdiBitmapData,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create bitmap from HBITMAP.
		/// </summary>
		/// <param name="hbm">The HBM.</param>
		/// <param name="hpal">The hpal.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromHBITMAP
			(
			IntPtr hbm,
			IntPtr hpal,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create HBITMAP from bitmap.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="hbmReturn">The HBM return.</param>
		/// <param name="background">The background.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateHBITMAPFromBitmap
			(
			IntPtr bitmap,
			IntPtr hbmReturn,
			Color background
			);

		/// <summary>
		/// Gdips the create bitmap from HICON.
		/// </summary>
		/// <param name="hicon">The hicon.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromHICON
			(
			IntPtr hicon,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the create HICON from bitmap.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="hbmReturn">The HBM return.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateHICONFromBitmap
			(
			IntPtr bitmap,
			out IntPtr hbmReturn
			);

		/// <summary>
		/// Gdips the create bitmap from resource.
		/// </summary>
		/// <param name="hInstance">The h instance.</param>
		/// <param name="lpBitmapName">Name of the lp bitmap.</param>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateBitmapFromResource
			(
			IntPtr hInstance,
			string lpBitmapName,
			out IntPtr bitmap
			);

		/// <summary>
		/// Gdips the clone bitmap area.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="format">The format.</param>
		/// <param name="srcBitmap">The SRC bitmap.</param>
		/// <param name="dstBitmap">The DST bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCloneBitmapArea
			(
			float x,
			float y,
			float width,
			float height,
			PixelFormat format,
			IntPtr srcBitmap,
			out IntPtr dstBitmap
			);

		/// <summary>
		/// Gdips the clone bitmap area I.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="format">The format.</param>
		/// <param name="srcBitmap">The SRC bitmap.</param>
		/// <param name="dstBitmap">The DST bitmap.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCloneBitmapAreaI
			(
			int x,
			int y,
			int width,
			int height,
			PixelFormat format,
			IntPtr srcBitmap,
			out IntPtr dstBitmap
			);

		/// <summary>
		/// Gdips the bitmap lock bits.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="rect">The rect.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="format">The format.</param>
		/// <param name="lockedBitmapData">The locked bitmap data.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipBitmapLockBits
			(
			IntPtr bitmap,
			ref Rectangle rect,
			int flags,
			PixelFormat format,
			IntPtr lockedBitmapData
			);

		/// <summary>
		/// Gdips the bitmap unlock bits.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="lockedBitmapData">The locked bitmap data.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipBitmapUnlockBits
			(
			IntPtr bitmap,
			IntPtr lockedBitmapData
			);

		/// <summary>
		/// Gdips the bitmap get pixel.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipBitmapGetPixel
			(
			IntPtr bitmap,
			int x,
			int y,
			out Color color
			);

		/// <summary>
		/// Gdips the bitmap set pixel.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipBitmapSetPixel
			(
			IntPtr bitmap,
			int x,
			int y,
			Color color
			);

		/// <summary>
		/// Gdips the bitmap set resolution.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <param name="xdpi">The xdpi.</param>
		/// <param name="ydpi">The ydpi.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipBitmapSetResolution
			(
			IntPtr bitmap,
			float xdpi,
			float ydpi
			);

		#endregion

		#region Graphics APIs

		/// <summary>
		/// Gdips the flush.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="intention">The intention.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipFlush
			(
			IntPtr graphics,
			FlushIntention intention
			);

		/// <summary>
		/// Gdips the create from IntPtr .
		/// </summary>
		/// <param name="hdc">The IntPtr .</param>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFromHDC
			(
			IntPtr hdc,
			out IntPtr graphics
			);

		/// <summary>
		/// Gdips the create from HD c2.
		/// </summary>
		/// <param name="hdc">The IntPtr .</param>
		/// <param name="hDevice">The h device.</param>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFromHDC2
			(
			IntPtr hdc,
			IntPtr hDevice,
			out IntPtr graphics
			);

		/// <summary>
		/// Gdips the create from HWND.
		/// </summary>
		/// <param name="hwnd">The HWND.</param>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFromHWND
			(
			IntPtr hwnd,
			out IntPtr graphics
			);

		/// <summary>
		/// Gdips the create from HWNDICM.
		/// </summary>
		/// <param name="hwnd">The HWND.</param>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFromHWNDICM
			(
			IntPtr hwnd,
			out IntPtr graphics
			);

		/// <summary>
		/// Gdips the delete graphics.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipDeleteGraphics
			(
			IntPtr graphics
			);

		/// <summary>
		/// Gdips the get DC.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="hdc">The IntPtr .</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetDC
			(
			IntPtr graphics,
			out IntPtr hdc
			);

		/// <summary>
		/// Gdips the release DC.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="hdc">The IntPtr .</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipReleaseDC
			(
			IntPtr graphics,
			IntPtr hdc
			);

		/// <summary>
		/// Gdips the set compositing mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="compositingMode">The compositing mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetCompositingMode
			(
			IntPtr graphics,
			CompositingMode compositingMode
			);

		/// <summary>
		/// Gdips the get compositing mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="compositingMode">The compositing mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetCompositingMode
			(
			IntPtr graphics,
			out CompositingMode compositingMode
			);

		/// <summary>
		/// Gdips the set rendering origin.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetRenderingOrigin
			(
			IntPtr graphics,
			int x,
			int y
			);

		/// <summary>
		/// Gdips the get rendering origin.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetRenderingOrigin
			(
			IntPtr graphics,
			out int x,
			out int y
			);

		/// <summary>
		/// Gdips the set compositing quality.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="compositingQuality">The compositing quality.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetCompositingQuality
			(
			IntPtr graphics,
			CompositingQuality compositingQuality
			);

		/// <summary>
		/// Gdips the get compositing quality.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="compositingQuality">The compositing quality.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetCompositingQuality
			(
			IntPtr graphics,
			out CompositingQuality compositingQuality
			);

		/// <summary>
		/// Gdips the set smoothing mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="smoothingMode">The smoothing mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetSmoothingMode
			(
			IntPtr graphics,
			SmoothingMode smoothingMode
			);

		/// <summary>
		/// Gdips the get smoothing mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="smoothingMode">The smoothing mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetSmoothingMode
			(
			IntPtr graphics,
			out SmoothingMode smoothingMode
			);

		/// <summary>
		/// Gdips the set pixel offset mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="pixelOffsetMode">The pixel offset mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetPixelOffsetMode
			(
			IntPtr graphics,
			PixelOffsetMode pixelOffsetMode
			);

		/// <summary>
		/// Gdips the get pixel offset mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="pixelOffsetMode">The pixel offset mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetPixelOffsetMode
			(
			IntPtr graphics,
			out PixelOffsetMode pixelOffsetMode
			);

		/// <summary>
		/// Gdips the set text rendering hint.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetTextRenderingHint
			(
			IntPtr graphics,
			TextRenderingHint mode
			);

		/// <summary>
		/// Gdips the get text rendering hint.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetTextRenderingHint
			(
			IntPtr graphics,
			out TextRenderingHint mode
			);

		/// <summary>
		/// Gdips the set text contrast.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="contrast">The contrast.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetTextContrast
			(
			IntPtr graphics,
			int contrast
			);

		/// <summary>
		/// Gdips the get text contrast.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="contrast">The contrast.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetTextContrast
			(
			IntPtr graphics,
			out int contrast
			);

		/// <summary>
		/// Gdips the set interpolation mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="interpolationMode">The interpolation mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetInterpolationMode
			(
			IntPtr graphics,
			InterpolationMode interpolationMode
			);

		/// <summary>
		/// Gdips the get interpolation mode.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="interpolationMode">The interpolation mode.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetInterpolationMode
			(
			IntPtr graphics,
			out InterpolationMode interpolationMode
			);

		/// <summary>
		/// Gdips the set world transform.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="matrix">The matrix.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipSetWorldTransform
			(
			IntPtr graphics,
			IntPtr matrix
			);

		/// <summary>
		/// Gdips the reset world transform.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipResetWorldTransform
			(
			IntPtr graphics
			);

		#endregion

		#region Font APIs

		/// <summary>
		/// Gdips the create font from DC.
		/// </summary>
		/// <param name="hdc">The HDC.</param>
		/// <param name="font">The font.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFontFromDC
			(
			IntPtr hdc,
			out IntPtr font
			);

		/// <summary>
		/// Gdips the create font from logfont A.
		/// </summary>
		/// <param name="hdc">The HDC.</param>
		/// <param name="logfont">The logfont.</param>
		/// <param name="font">The font.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFontFromLogfontA
			(
			IntPtr hdc,
			ref LOGFONT logfont,
			out IntPtr font
			);

		/// <summary>
		/// Gdips the create font from logfont W.
		/// </summary>
		/// <param name="hdc">The HDC.</param>
		/// <param name="logfont">The logfont.</param>
		/// <param name="font">The font.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFontFromLogfontW
			(
			IntPtr hdc,
			IntPtr logfont,
			out IntPtr font
			);

		/// <summary>
		/// Gdips the create font.
		/// </summary>
		/// <param name="fontFamily">The font family.</param>
		/// <param name="emSize">Size of the em.</param>
		/// <param name="style">The style.</param>
		/// <param name="unit">The unit.</param>
		/// <param name="font">The font.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCreateFont
			(
			FontFamily fontFamily,
			float emSize,
			int style,
			GraphicsUnit unit,
			out IntPtr font
			);

		/// <summary>
		/// Gdips the clone font.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="cloneFont">The clone font.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipCloneFont
			(
			IntPtr font,
			out IntPtr cloneFont
			);

		/// <summary>
		/// Gdips the delete font.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipDeleteFont
			(
			IntPtr font
			);

		/// <summary>
		/// Gdips the get family.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="family">The family.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFamily
			(
			IntPtr font,
			out FontFamily family
			);

		/// <summary>
		/// Gdips the get font style.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="style">The style.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontStyle
			(
			IntPtr font,
			out int style
			);

		/// <summary>
		/// Gdips the size of the get font.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontSize
			(
			IntPtr font,
			out float size
			);

		/// <summary>
		/// Gdips the get font unit.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="unit">The unit.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontUnit
			(
			IntPtr font,
			out GraphicsUnit unit
			);

		/// <summary>
		/// Gdips the height of the get font.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="graphics">The graphics.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontHeight
			(
			IntPtr font,
			IntPtr graphics,
			out float height
			);

		/// <summary>
		/// Gdips the get font height given DPI.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="dpi">The dpi.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontHeightGivenDPI
			(
			IntPtr font,
			float dpi,
			out float height
			);

		/// <summary>
		/// Gdips the get log font A.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="graphics">The graphics.</param>
		/// <param name="logfontA">The logfont A.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetLogFontA
			(
			IntPtr font,
			IntPtr graphics,
			out LOGFONT logfontA
			);

		/// <summary>
		/// Gdips the get log font W.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="graphics">The graphics.</param>
		/// <param name="logfontW">The logfont W.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetLogFontW
			(
			IntPtr font,
			IntPtr graphics,
			IntPtr logfontW
			);

		/// <summary>
		/// Gdips the new installed font collection.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipNewInstalledFontCollection
			(
			out IntPtr fontCollection
			);

		/// <summary>
		/// Gdips the new private font collection.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipNewPrivateFontCollection
			(
			out IntPtr fontCollection
			);

		/// <summary>
		/// Gdips the delete private font collection.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipDeletePrivateFontCollection
			(
			IntPtr fontCollection
			);

		/// <summary>
		/// Gdips the get font collection family count.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <param name="numFound">The num found.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontCollectionFamilyCount
			(
			IntPtr fontCollection,
			out int numFound
			);

		/// <summary>
		/// Gdips the get font collection family list.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <param name="numSought">The num sought.</param>
		/// <param name="gpfamilies">The gpfamilies.</param>
		/// <param name="numFound">The num found.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipGetFontCollectionFamilyList
			(
			IntPtr fontCollection,
			int numSought,
			ref IntPtr gpfamilies,
			out int numFound
			);

		/// <summary>
		/// Gdips the private add font file.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipPrivateAddFontFile
			(
			IntPtr fontCollection,
			string filename
			);

		/// <summary>
		/// Gdips the private add memory font.
		/// </summary>
		/// <param name="fontCollection">The font collection.</param>
		/// <param name="memory">The memory.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdipPrivateAddMemoryFont
			(
			IntPtr fontCollection,
			IntPtr memory,
			int length
			);

		#endregion

		#region Hook APIs

		/// <summary>
		/// Gdipluses the notification hook.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		[DllImport ( DllName )]
		public static extern GdiPlusStatus GdiplusNotificationHook
			(
			out IntPtr token
			);

		/// <summary>
		/// Gdipluses the notification unhook.
		/// </summary>
		/// <param name="token">The token.</param>
		[DllImport ( DllName )]
		public static extern void GdiplusNotificationUnhook
			(
			IntPtr token
			);

		#endregion

		#region Other

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hEmf"></param>
		/// <param name="bufferSize"></param>
		/// <param name="buffer"></param>
		/// <param name="mappingMode"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		[DllImport ( DllName, ExactSpelling=true )]
		public static extern int GdipEmfToWmfBits
			(
				IntPtr hEmf,
				int bufferSize,
				byte [] buffer,
				MapMode mappingMode,
				EmfToWmfBitsFlags flags
			);

		#endregion

		#endregion
	}
}