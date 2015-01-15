using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Common.Tools.IO;
using System.Configuration;

namespace Common.Tools.Media
{
    public partial class ImageHelper
    {
        public static string GetQualifiedMediaURL(string RelativePath)
        {
            if (string.IsNullOrEmpty(RelativePath))
                RelativePath = "/SMART/images/pageContent/photocomingsoon_square.gif";

            string qualifiedFilePath = RelativePath.Replace("\\", "/");

            //TODO:  Currently the thumbnail paths on MFs contain /SMART/ so we need this if statement
            //Once we change publishing to not contain SMART then this code may need to change
            if (RelativePath.ToLower().StartsWith("/smart/") || RelativePath.ToLower().StartsWith("/iconoiq/") || RelativePath.ToLower().StartsWith("/media/"))
            {
                //Add Domain
                qualifiedFilePath = ConfigurationManager.AppSettings["g_Domain"] + qualifiedFilePath;
            }
            else
            {
                //Add Media directory path
                qualifiedFilePath = ConfigurationManager.AppSettings["g_MediaDirectory"] + qualifiedFilePath;
            }

            return qualifiedFilePath;
        }

        public static string GetRelativeMediaURL(string RelativePath)
        {
            string strRelativeMediaURL = string.Empty;

            if (RelativePath.ToLower().Contains(ConfigurationManager.AppSettings["g_MediaDirectory"]))
            {
                strRelativeMediaURL += RelativePath.Replace("\\", "/");
            }
            else
            {
                strRelativeMediaURL = "/Media/" + RelativePath.Replace("\\", "/");
            }

            return strRelativeMediaURL;
        }


        /// <summary>
        /// ResizeImage will fit the image into a "box" of MaxWidth and MaxHeight
        /// </summary>
        /// <param name="FileBytes">original file to be resized</param>
        /// <param name="MaxWidth">Max width of the new image</param>
        /// <param name="MaxHeight">Max height of the new image</param>
        /// <param name="MaintainAspectRatio">If selected, the image will not be cropped, but instead resized to fit into the "box"</param>
        /// <returns></returns>
        public static byte[] ResizeImage(Image OriginalImage, int MaxWidth, int MaxHeight, bool MaintainAspectRatio)
        {
            int intDestWidth = MaxWidth;
            int intDestHeight = MaxHeight;

            if (MaintainAspectRatio)
            {
                //Force the full image into the "box" (e.g. Thumbnail's can have a max of 100x100 but should maintain aspect ratio)
                float nWidthReducePercentage = ((float)MaxWidth / (float)OriginalImage.Width);
                float nHeightReducePercentage = ((float)MaxHeight / (float)OriginalImage.Height);

                //We will use the smaller percentage to resize the picture
                if (nWidthReducePercentage < nHeightReducePercentage)
                {
                    intDestWidth = MaxWidth;
                    intDestHeight = (int)(OriginalImage.Height * nWidthReducePercentage);
                }
                else
                {
                    intDestHeight = MaxHeight;
                    intDestWidth = (int)(OriginalImage.Width * nHeightReducePercentage);
                }
            }
            else
            {
                //Crop image before fitting into the new size
                OriginalImage = CropImage(OriginalImage, MaxWidth, MaxHeight);
            }

            Bitmap objBitmap = new Bitmap(intDestWidth, intDestHeight);

            Graphics graphics = Graphics.FromImage(objBitmap);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(OriginalImage, 0, 0, intDestWidth, intDestHeight);

            MemoryStream msDerivedImage = new MemoryStream();
            objBitmap.Save(msDerivedImage, ImageFormat.Png);

            graphics.Dispose();
            objBitmap.Dispose();

            return msDerivedImage.ToArray();
        }

        public static byte[] ConvertImageToByteArray(Image ImageFile)
        {
            MemoryStream ms = new MemoryStream();
            ImageFile.Save(ms, ImageFile.RawFormat);

            byte[] ImageBytes = ms.ToArray();
            ms.Close();

            return ImageBytes;

        }

        public static Image ConvertByteArrayToImage(byte[] ImageBytes)
        {
            MemoryStream ms = new MemoryStream(ImageBytes);
            Image OriginalImage = Image.FromStream(ms);

            ms.Close();

            return OriginalImage;
        }

        public static FileInfo SaveImageToFileSystem(Image Image, string FilePath)
        {
            byte[] aImageBytes = ImageHelper.ConvertImageToByteArray(Image);

            FileInfo objImageFileInfo = FileHelper.SaveToFileSystem(FilePath, aImageBytes);

            return objImageFileInfo;
        }

        #region "Private Static Methods"

        private static Image CropImage(Image OriginalImage, int MaxWidth, int MaxHeight)
        {
            Rectangle objCropArea = CreateCropArea(OriginalImage, MaxWidth, MaxHeight);

            Bitmap bmpImage = new Bitmap(OriginalImage);

            Bitmap bmpCrop = bmpImage.Clone(objCropArea, bmpImage.PixelFormat);

            return (Image)(bmpCrop);
        }

        private static Rectangle CreateCropArea(Image OriginalImage, int MaxWidth, int MaxHeight)
        {
            int intPositionX = 0;
            int intPositionY = 0;

            int intDestWidth = 0;
            int intDestHeight = 0;

            float nRatioW = ((float)OriginalImage.Width / (float)MaxWidth);
            float nRatioH = ((float)OriginalImage.Height / (float)MaxHeight);

            //We will fit the smaller edge (width or height) to the new size and then crop the other direction
            if (nRatioW > nRatioH)
            {
                //height ratio is smaller so we will base off that. Crop the width
                intDestHeight = OriginalImage.Height;
                intDestWidth = Convert.ToInt32((float)OriginalImage.Width * (nRatioH / nRatioW));

                //Figure out how many pixels need to be removed
                int intHorizontalPixelsToRemove = OriginalImage.Width - intDestWidth;
                intPositionX = Convert.ToInt32((float)intHorizontalPixelsToRemove / 2);

            }
            else
            {
                //width ratio is smaller so we will base off that. Crop the height
                intDestWidth = OriginalImage.Width;
                intDestHeight = Convert.ToInt32((float)OriginalImage.Height * (nRatioW / nRatioH));


                //Figure out how many pixels need to be removed
                int intVerticalPixelsToRemove = OriginalImage.Height - intDestHeight;
                intPositionY = Convert.ToInt32((float)intVerticalPixelsToRemove / 2);
            }

            Rectangle objCropArea = new Rectangle(intPositionX, intPositionY, intDestWidth, intDestHeight);

            return objCropArea;
        }


        #endregion
    }
}
