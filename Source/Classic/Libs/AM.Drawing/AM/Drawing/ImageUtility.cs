/* ImageUtility.cs -- image manipulation helpers
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// Image manipulation helpers.
    /// </summary>
    [PublicAPI]
    public static class ImageUtility
    {
        #region Public methods

        /// <summary>
        /// Get codec info.
        /// </summary>
        /// <param name="mimeType">For example "image/jpeg".
        /// </param>
        /// <returns>ImageCodeInfo or null.</returns>
        [CanBeNull]
        public static ImageCodecInfo GetCodecInfo
            (
                [NotNull] string mimeType
            )
        {
            Code.NotNullNorEmpty(mimeType, "mimeType");

            ImageCodecInfo[] codecs
                = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }

            return null;
        }

        /// <summary>
        /// Сохраняет картинку в памяти.
        /// </summary>
        [NotNull]
        public static byte[] SaveToMemory
            (
                [NotNull] Image image,
                [NotNull] ImageFormat format
            )
        {
            MemoryStream memory = new MemoryStream();
            image.Save(memory, format);

            return memory.ToArray();
        }

        /// <summary>
        /// Загружает картинку из памяти.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image LoadFromMemory(byte[] bytes)
        {
            Stream memory = new MemoryStream(bytes, false);
            return Image.FromStream(memory);
        }

        /// <summary>
        /// Загружаем картинку из файла.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks>Этот нехитрый трюк нужен, чтобы не блокировать
        /// файл, как это обычно делает System.Drawing.Image.FromFile().
        /// </remarks>
        public static Image LoadFromFile
            (
                [NotNull] string fileName
            )
        {
            Stream memory = new MemoryStream
                (
                    File.ReadAllBytes(fileName),
                    false
                );

            return Image.FromStream(memory);
        }

        /// <summary>
        /// Загружает картинку из ресурсов .NET.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Image LoadFromResource
            (
                Assembly assembly,
                string resourceName
            )
        {
            using (Stream stream
                = assembly.GetManifestResourceStream(resourceName))
            {
                return Image.FromStream(stream);
            }
        }

        /// <summary>
        /// Загружает картинку из ресурсов .NET.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Image LoadFromResource(Type type,
            string resourceName)
        {
            return LoadFromResource(type.Assembly, resourceName);
        }

        /// <summary>
        /// Пропорционально масштабирует изображение так, чтобы оно
        /// вписывалось в указанные размеры.
        /// </summary>
        public static Image ProportionalResize
            (
                Image image,
                int width,
                int height
            )
        {
            double imageHeight = image.Height;
            double imageWidth = image.Width;
            double windowHeight = width;
            double windowWidth = height;
            double imageAspect = imageWidth / imageHeight;
            double panelAspect = windowWidth / windowHeight;
            double superAspect = imageAspect / panelAspect;
            double ratio = (superAspect > 1.0)
                ? windowWidth / imageWidth
                : windowHeight / imageHeight;
            imageWidth *= ratio;
            imageHeight *= ratio;
            Bitmap result = new Bitmap(image, (int)imageWidth,
                (int)imageHeight);

            return result;
        }

        /// <summary>
        /// Save bitmap in JPEG file with given quality.
        /// </summary>
        public static void SaveJpeg
            (
                Image img,
                string fileName,
                long quality
            )
        {
            ImageCodecInfo ici = GetCodecInfo("image/jpeg");
            EncoderParameter par0 = new EncoderParameter(Encoder.Quality,
                quality);
            EncoderParameters parms = new EncoderParameters(1);
            parms.Param = new EncoderParameter[] {
                par0
            };
            img.Save(fileName, ici, parms);
        }

        /// <summary>
        /// Получение копии рисунка с исправленной гаммой.
        /// </summary>
        public static Image ReGamma
            (
                Image image,
                float gamma
            )
        {
            return ReGamma
                (
                    image,
                    new Rectangle
                    (
                        0,
                        0,
                        image.Width,
                        image.Height
                    ),
                    gamma
                );
        }

        /// <summary>
        /// Получение копии рисунка с исправленной гаммой.
        /// </summary>
        public static Image ReGamma
            (
                Image image,
                Rectangle dstRect,
                float gamma
            )
        {
            Image result = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(result))
            using (ImageAttributes attr = new ImageAttributes())
            {
                attr.SetGamma(gamma, ColorAdjustType.Bitmap);
                g.DrawImage(image, dstRect,
                    0f, 0f, image.Width, image.Height,
                    GraphicsUnit.Pixel, attr);
            }

            return result;
        }

        ///// <summary>
        ///// Получение всей поверхности экрана (игнорируя окна) для рисования.
        ///// </summary>
        ///// <returns></returns>
        //public static Graphics GetWholeScreen ()
        //{
        //    IntPtr hdc = AM.Win32.User32.GetDC ( IntPtr.Zero );
        //    return Graphics.FromHdc ( hdc );
        //}

        #endregion
    }
}
