// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ImageInfo.cs -- general information about image
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// General information about image.
    /// </summary>
    [PublicAPI]
    public sealed class ImageInfo
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const int Unspecified = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the colors.
        /// </summary>
        public long Colors { get; internal set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string FileName { get; internal set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Gets the horizontal resolution.
        /// </summary>
        public double HorizontalResolution { get; internal set; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public long Length { get; internal set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version { get; internal set; }

        /// <summary>
        /// Gets the vertical resolution.
        /// </summary>
        public double VerticalResolution { get; internal set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageInfo"/> class.
        /// </summary>
        internal ImageInfo()
        {
            _Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageInfo"/> class.
        /// </summary>
        internal ImageInfo(string fileName)
        {
            _Clear();

            FileName = fileName;
            Length = new FileInfo(fileName).Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageInfo"/> class.
        /// </summary>
        internal ImageInfo
            (
                int width,
                int height
            )
        {
            _Clear();

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ImageInfo"/> class.
        /// </summary>
        internal ImageInfo
            (
                int width,
                int height,
                long colors
            )
        {
            _Clear();

            Width = width;
            Height = height;
            Colors = colors;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        private void _Clear()
        {
            Colors = Unspecified;
            Height = Unspecified;
            Length = Unspecified;
            Width = Unspecified;
        }

        private static bool _Compare
            (
                byte[] stream,
                int offset,
                params byte[] pattern
            )
        {
            if ((offset + pattern.Length) > stream.Length)
            {
                return false;
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                if (stream[offset + i] != pattern[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static ImageInfo _GetBmpImageInfo
            (
                string fileName
            )
        {
            using (FileStream file = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[256];
                ImageInfo result = new ImageInfo(fileName);
                while (true)
                {
                    if ((file.Read(buffer, 0, 32) != 32)
                       || !_Compare(buffer, 0, 0x42, 0x4D)
                       )
                    {
                        break;
                    }
                    result.Width = BitConverter.ToInt32(buffer, 18);
                    result.Height = BitConverter.ToInt32(buffer, 22);
                    result.Colors = 1 << buffer[28];

                    return result;
                }

                throw new ApplicationException();
            }
        }

        private static ImageInfo _GetGifImageInfo
            (
                string fileName
            )
        {
            using (FileStream file = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[256];
                ImageInfo result = new ImageInfo(fileName);
                while (true)
                {
                    if ((file.Read(buffer, 0, 13) != 13)
                       || !_Compare(buffer, 0, 0x47, 0x49, 0x46)
                       )
                    {
                        break;
                    }
                    string version = Encoding.ASCII.GetString(buffer, 3, 3);
                    if ((version != "87a") && (version != "89a"))
                    {
                        break;
                    }
                    result.Version = version;
                    result.Width = buffer[6] + buffer[7] * 256;
                    result.Height = buffer[8] + buffer[9] * 256;
                    byte packed = buffer[10];
                    result.Colors = 1 << ((packed & 7) + 1);

                    return result;
                }

                throw new ApplicationException();
            }
        }

        private static ImageInfo _GetJpegImageInfo
            (
                string fileName
            )
        {
            using (FileStream file = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[256];
                if ((file.Read(buffer, 0, 2) != 2)
                   || !_Compare(buffer, 0, 0xFF, 0xD8)
                   )
                {
                    throw new ApplicationException();
                }

                ImageInfo result = new ImageInfo(fileName);
                result.Colors = 1 << 24;
                while (true)
                {
                    if ((file.Read(buffer, 0, 2) != 2)
                       || (buffer[0] != 0xFF)
                       )
                    {
                        break;
                    }
                    long position = file.Position;
                    byte blockCode = buffer[1];
                    int blockLength = _ReadUInt16(file);
                    if (Utility.IsOneOf<byte>(blockCode, 0xE0))
                    {
                        int toRead = Math.Min(blockLength, buffer.Length);
                        if ((file.Read(buffer, 0, toRead) != toRead)
                           || !_Compare(buffer, 0, 0x4A, 0x46, 0x49, 0x46, 0x00)
                           )
                        {
                            break;
                        }
                        result.Version
                            = new Version(buffer[5], buffer[6]).ToString();
                        result.HorizontalResolution
                            = (short)(buffer[8] * 256 + buffer[9]);
                        result.VerticalResolution
                            = (short)(buffer[10] * 256 + buffer[11]);
                    }
                    if (Utility.IsOneOf<byte>(blockCode, 0xC0, 0xC1,
                        0xC2, 0xC3, 0xC5, 0xC6, 0xC7, 0xC9, 0xCA, 0xCB,
                        0xCD, 0xCE, 0xCF))
                    {
                        if (file.ReadByte() < 0)
                        {
                            throw new IOException();
                        }
                        result.Height = _ReadUInt16(file);
                        result.Width = _ReadUInt16(file);

                        return result;
                    }
                    file.Position = position + blockLength;
                    continue;
                }
            }

            throw new ApplicationException();
        }

        private static ImageInfo _GetPcxImageInfo
            (
                string fileName
            )
        {
            throw new NotImplementedException(nameof(_GetPcxImageInfo));
        }

        private static ImageInfo _GetTgaImageInfo
            (
                string fileName
            )
        {
            throw new NotImplementedException(nameof(_GetTgaImageInfo));
        }

        private static ImageInfo _GetTiffImageInfo
            (
                string fileName
            )
        {
            throw new NotImplementedException(nameof(_GetTiffImageInfo));
        }

        private static ushort _ReadUInt16(Stream stream)
        {
            byte[] buffer = new byte[2];
            int readed = stream.Read(buffer, 0, 2);
            if (readed != 2)
            {
                throw new IOException();
            }
            return (ushort)(buffer[0] * 256 + buffer[1]);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the image info.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static ImageInfo FromFile
            (
                string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string extension = Path.GetExtension(fileName)
                .ThrowIfNull("extension")
                .ToLowerInvariant();
            switch (extension)
            {
                case ".bmp":
                    return _GetBmpImageInfo(fileName);

                case ".gif":
                    return _GetGifImageInfo(fileName);

                case ".jpeg":
                case ".jpg":
                case ".jfif":
                    return _GetJpegImageInfo(fileName);

                case ".pcx":
                    return _GetPcxImageInfo(fileName);

                case ".tga":
                    return _GetTgaImageInfo(fileName);

                case ".tif":
                case ".tiff":
                    return _GetTiffImageInfo(fileName);

                default:
                    throw new ArgumentException();
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Width={0} Height={1} Colors={2}",
                    Width,
                    Height,
                    Colors
                );
        }

        #endregion
    }
}
