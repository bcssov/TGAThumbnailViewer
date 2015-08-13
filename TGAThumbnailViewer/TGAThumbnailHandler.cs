// ***********************************************************************
// Assembly         : TGAThumbnailViewer
// Author           : Mario
// Created          : 08-13-2015
//
// Last Modified By : Mario
// Last Modified On : 08-13-2015
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
        #region Fields

        /// <summary>
        /// The _empty
        /// </summary>
        private Bitmap _empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TGAThumbnailHandler" /> class.
        /// </summary>
        public TGAThumbnailHandler()
        {
            _empty = CreateEmptyBitmap();
        }

        #endregion Constructors

        #region Methods

        // source: https://stackoverflow.com/questions/8214562/resize-jpeg-image-to-fixed-width-while-keeping-aspect-ratio-as-it-is
        /// <summary>
        /// Resizes the width of the image fixed.
        /// </summary>
        /// <param name="imgToResize">The img to resize.</param>
        /// <param name="width">The width.</param>
        /// <returns>Image.</returns>
        public Bitmap ResizeImage(Image imgToResize, int width)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = ((float)width / (float)sourceWidth);

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        /// <summary>
        /// Gets the thumbnail image.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>Bitmap.</returns>
        protected override Bitmap GetThumbnailImage(uint width)
        {
            try
            {
                var tga = new Paloma.TargaImage(SelectedItemStream);
                var img = tga.Image;
                if (img != null)
                    return ResizeImage(img, Convert.ToInt32(width));
            }
            catch (Exception ex)
            {
                LogError("TGAThumbnailViewer", ex);
            }
            return _empty;
        }

        /// <summary>
        /// Creates the empty bitmap.
        /// </summary>
        /// <returns>Bitmap.</returns>
        private Bitmap CreateEmptyBitmap()
        {
            Bitmap bmp = new Bitmap(100, 100);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 100, 100);
                graph.FillRectangle(Brushes.White, ImageSize);
            }
            return bmp;
        }

        #endregion Methods
    }
}