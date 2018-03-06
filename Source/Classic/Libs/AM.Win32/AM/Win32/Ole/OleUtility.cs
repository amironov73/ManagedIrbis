// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OleUtility.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;

using AM.Runtime;

using CodeJam;

//using EnvDTE;

using IDataObject = System.Windows.Forms.IDataObject;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    public static class OleUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        public static Bitmap GetBitmap
            (
                IDataObject dataObject
            )
        {
            Code.NotNull(dataObject, "dataObject");

            object oleData = dataObject.GetData(DataFormats.Dib);
            MemoryStream stream = oleData as MemoryStream;
            Bitmap result = null;
            if (stream != null)
            {
                byte[] bytes = stream.ToArray();
                IntPtr dibptr = InteropUtility.BufferToPtr(bytes);
                BITMAPINFO bmi = (BITMAPINFO)
                                 Marshal.PtrToStructure(dibptr, typeof(BITMAPINFO));
                IntPtr pixelData = BitmapUtility.GetPixelData(dibptr);
                IntPtr bitmap;
                GdiPlusStatus retcode = GdiPlus.GdipCreateBitmapFromGdiDib
                    (
                        ref bmi,
                        pixelData,
                        out bitmap
                    );
                if ((retcode == GdiPlusStatus.Ok)
                     && (bitmap != IntPtr.Zero))
                {
                    result = GdiPlusUtility.GetBitmapFromGdiPlus(bitmap);
                }
            }

            return result;
        }

        ///// <summary>
        ///// Gets the DTE.
        ///// </summary>
        ///// <param name="processID">The process ID.</param>
        ///// <returns></returns>
        //[CLSCompliant(false)]
        //public static DTE GetDTE(string processID)
        //{
        //    IRunningObjectTable prot;
        //    IEnumMoniker pMonkEnum;

        //    string progID = "!VisualStudio.DTE.8.0:" + processID;
        //    Ole32.GetRunningObjectTable(0, out prot);
        //    prot.EnumRunning(out pMonkEnum);
        //    pMonkEnum.Reset();

        //    IntPtr fetched = IntPtr.Zero;
        //    IMoniker[] pmon = new IMoniker[1];
        //    while (pMonkEnum.Next(1, pmon, fetched) == 0)
        //    {
        //        IBindCtx pCtx;
        //        Ole32.CreateBindCtx(0, out pCtx);
        //        string str;
        //        pmon[0].GetDisplayName(pCtx, null, out str);
        //        if (str == progID)
        //        {
        //            object objReturnObject;
        //            prot.GetObject(pmon[0], out objReturnObject);
        //            DTE ide = (DTE)objReturnObject;
        //            return ide;
        //        }
        //    }

        //    return null;
        //}


        /// <summary>
        /// Gets the file group A.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        public static FILEGROUPDESCRIPTORA? GetFileGroupA
            (
                IDataObject dataObject
            )
        {
            object oleData
                = dataObject.GetData(AdditionalDataFormats.FileGroupDescriptor);
            MemoryStream stream = oleData as MemoryStream;
            if (stream != null)
            {
                byte[] bytes = stream.ToArray();
                IntPtr ptr = InteropUtility.BufferToPtr(bytes);
                FILEGROUPDESCRIPTORA result = (FILEGROUPDESCRIPTORA)
                                              Marshal.PtrToStructure(ptr,
                                                                       typeof(
                                                                           FILEGROUPDESCRIPTORA
                                                                           ));
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="oleData">The OLE data.</param>
        /// <returns></returns>
        public static string GetText
            (
                IDataObject oleData
            )
        {
            return GetText
                       (
                           oleData,
                           DataFormats.UnicodeText,
                           Encoding.Unicode
                       )
                   ??
                   GetText
                       (
                           oleData,
                           DataFormats.Text,
                           Encoding.Default
                       );
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="oleData">The OLE data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GetText
            (
                object oleData,
                Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            string result = oleData as string;
            if (result == null)
            {
                MemoryStream stream = oleData as MemoryStream;
                if (stream != null)
                {
                    byte[] bytes = stream.ToArray();
                    result = encoding.GetString(bytes).TrimEnd('\0');
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="dataFormat">The data format.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GetText
            (
                IDataObject dataObject,
                string dataFormat,
                Encoding encoding
            )
        {
            Code.NotNull(dataObject, "dataObject");
            Code.NotNullNorEmpty(dataFormat, "dataFormat");
            Code.NotNull(encoding, "encoding");

            string result = null;
            object oleData = dataObject.GetData(dataFormat, true);
            if (dataFormat != null)
            {
                result = GetText(oleData, encoding);
            }

            return result;
        }

        /// <summary>
        /// Gets the uniform resource locator.
        /// </summary>
        /// <param name="oleData">The OLE data.</param>
        /// <returns></returns>
        public static string GetUniformResourceLocator(IDataObject oleData)
        {
            string result = GetText
                                (
                                oleData,
                                AdditionalDataFormats.UniformResourceLocatorW,
                                Encoding.Unicode
                                )
                            ??
                            GetText
                                (
                                oleData,
                                AdditionalDataFormats.UniformResourceLocator,
                                Encoding.ASCII
                                );
            return result;
        }

        #endregion
    }
}