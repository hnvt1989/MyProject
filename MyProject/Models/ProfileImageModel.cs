using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomizeIt.Models
{
    public class ProfileImageModel
    {
        public string DefaultImageData { get { return "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAIAAABMXPacAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyFpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNS1jMDE0IDc5LjE1MTQ4MSwgMjAxMy8wMy8xMy0xMjowOToxNSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo3MTBEMTMwRjY3NUExMUUzOTA1REU5RTk5MkVBNEFCMiIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDo3MTBEMTMxMDY3NUExMUUzOTA1REU5RTk5MkVBNEFCMiI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjcxMEQxMzBENjc1QTExRTM5MDVERTlFOTkyRUE0QUIyIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjcxMEQxMzBFNjc1QTExRTM5MDVERTlFOTkyRUE0QUIyIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+FjBxlQAAB7ZJREFUeNrsnVtPU0sU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"; } }


        #region Public Methods

        //public string ValidateImage(byte[] binary, string type, ProfileImageUploadOptions profileImageUploadOptions)
        //{
        //    if (binary.Length > profileImageUploadOptions.MaxImageSizeInBytes)
        //    {
        //        return "File size too large";
        //    }

        //    var goodExtension = profileImageUploadOptions.ImageExtensions.Count(ext => "image/" + ext == type) > 0;
        //    if (!goodExtension)
        //    {
        //        return "Invalid Extension. Must be one of the following: " +
        //               string.Join(", ", profileImageUploadOptions.ImageExtensions.ToArray()) + ".";
        //    }

        //    return null;
        //}

        public Tuple<TempImageSavedData, TempImageSavedData> ResizeImages(byte[] binary,
                                                                           ProfileImageUploadOptions profileImageUploadOptions)
        {
            var iconBinary = ResizeImage(binary, new RectangleF
            {
                Width = profileImageUploadOptions.IconWidth,
                Height = profileImageUploadOptions.IconHeight
            });

            var fullsizeBinary = ResizeImage(binary, new RectangleF
            {
                Width = profileImageUploadOptions.FullsizeWidth,
                Height = profileImageUploadOptions.FullsizeHeight
            });

            const string newType = "image/jpeg";

            // TODO: Rename this to something other than "fake." This is still used to preview images before saving.
            var iconResource = TempImageResource.SaveImage(GetTempIconResourceName(), newType, iconBinary);
            var fullsizeResource = TempImageResource.SaveImage(GetTempFullsizeResourceName(), newType,
                                                               fullsizeBinary);

            return new Tuple<TempImageSavedData, TempImageSavedData>(iconResource, fullsizeResource);
        }


        public void UpdateProfileImage(byte[] imageData, string contentType)
        {
            ServerInterface.UpdateProfileImage(CurrentPrimaryActivityCenterCode, imageData, contentType);
        }
        public void UpdateProfileThumbnailImage(byte[] imageData, string contentType)
        {
            ServerInterface.UpdateProfileImage(CurrentPrimaryActivityCenterCode + "_thumb", imageData, contentType);
        }

        public string GetTempIconResourceName()
        {
            return string.Format(CultureInfo.InvariantCulture, "TEMP_ICON.{0}.image", CurrentPrimaryActivityCenterCode);
        }

        public string GetTempFullsizeResourceName()
        {
            return string.Format(CultureInfo.InvariantCulture, "TEMP_FULL_SIZE.{0}.image", CurrentPrimaryActivityCenterCode);
        }

        public string GetFinalIconResourceName()
        {
            return string.Format(CultureInfo.InvariantCulture, "Final_ICON.{0}.image", CurrentPrimaryActivityCenterCode);
        }

        public string GetFinalFullsizeResourceName()
        {
            return string.Format(CultureInfo.InvariantCulture, "Final_Fullsize_ICON.{0}.image", CurrentPrimaryActivityCenterCode);
        }

        #endregion




        private static Byte[] ResizeImage(Byte[] src, RectangleF bounds)
        {
            var ms = new MemoryStream(src);
            var image = Image.FromStream(ms);

            ms = new MemoryStream();
            var newImage = ResizeImage(image, bounds);
            newImage.Save(ms, ImageFormat.Jpeg);
            var dest = ms.ToArray();
            return dest;
        }

        private static Image ResizeImage(Image source, RectangleF destinationBounds)
        {
            var sourceBounds = new RectangleF(0.0f, 0.0f, source.Width, source.Height);

            Image destinationImage = new Bitmap((int)destinationBounds.Width, (int)destinationBounds.Height);
            var graph = Graphics.FromImage(destinationImage);
            graph.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Fill with background color
            graph.FillRectangle(new SolidBrush(Color.White), destinationBounds);

            float resizeRatio;
            float scaleWidth, scaleHeight;
            float trimValue;
            float sourceRatio = source.Width / (float)source.Height;

            //if (sourceRatio >= 1.0f) //Flipped the logic to "fill" the space instead of "fitting"
            if (sourceRatio <= 1.0f)
            {
                //landscape
                resizeRatio = destinationBounds.Width / sourceBounds.Width;
                scaleWidth = destinationBounds.Width;
                scaleHeight = sourceBounds.Height * resizeRatio;
                trimValue = destinationBounds.Height - scaleHeight;
                graph.DrawImage(source, 0, (trimValue / 2), scaleWidth, scaleHeight);
            }
            else
            {
                //portrait
                resizeRatio = destinationBounds.Height / sourceBounds.Height;
                scaleWidth = sourceBounds.Width * resizeRatio;
                scaleHeight = destinationBounds.Height;
                trimValue = destinationBounds.Width - scaleWidth;
                graph.DrawImage(source, (trimValue / 2), 0, scaleWidth, scaleHeight);
            }

            return destinationImage;
        }


    }
}