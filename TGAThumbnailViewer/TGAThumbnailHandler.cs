// ***********************************************************************
// Assembly         : TGAThumbnailViewer
// Author           : Mario
// Created          : 08-13-2015
//
// Last Modified By : Mario
// Last Modified On : 08-15-2015
// ***********************************************************************
// <copyright file="TGAThumbnailHandler.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using SharpShell.Attributes;
using SharpShell.SharpThumbnailHandler;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

/// <summary>
/// The TGAThumbnailViewer namespace.
/// </summary>
namespace TGAThumbnailViewer
{
    /// <summary>
    /// Class TGAThumbnailHandler.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.FileExtension, ".tga")]
    public class TGAThumbnailHandler : SharpThumbnailHandler
    {
        #region Methods

        /// <summary>
        /// Creates the empty bitmap.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>Bitmap.</returns>
        protected Bitmap CreateEmptyBitmap(int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle imageSize = new Rectangle(0, 0, size, size);
                g.FillRectangle(Brushes.White, imageSize);
            }
            return bmp;
        }

        /// <summary>
        /// Gets the thumbnail image.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>Bitmap.</returns>
        protected override Bitmap GetThumbnailImage(uint width)
        {
            Bitmap bmp = null;
            using (SelectedItemStream)
            {
                try
                {
                    var tga = new Paloma.TargaImage(SelectedItemStream);
                    if (tga != null)
                    {
                        var tgaImg = tga.Image;
                        if (tgaImg != null)
                            bmp = ResizeImage(tgaImg, Convert.ToInt32(width));
                        tga.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    LogError("TGAThumbnailViewer", ex);
                }
            }
            if (bmp == null)
                bmp = CreateEmptyBitmap(Convert.ToInt32(width));
            GC.Collect();
            return bmp;
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="imgToResize">The img to resize.</param>
        /// <param name="width">The width.</param>
        /// <returns>Bitmap.</returns>
        protected Bitmap ResizeImage(Image imgToResize, int width)
        {
            // source: https://stackoverflow.com/questions/8214562/resize-jpeg-image-to-fixed-width-while-keeping-aspect-ratio-as-it-is
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = Convert.ToSingle(width) / Convert.ToSingle(sourceWidth);

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmp = new Bitmap(destWidth, destHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            }
            return bmp;
        }

        #endregion Methods
    }
}