using System;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace CMS.Web.Utilities
{
    public static class ImageHelpers
    {
        private const int DetailSizeWidth = 1280; // width
        private const int DetailSizeHeight = 1024; // height

        private const int ThumbnailSizeWidth = 320; // width
        private const int ThumbnailSizeHeight = 240; // height

        public static void ResizeImg(object data)
        {
            var (path, fileName) = (ValueTuple<string, string>)data;
            if (!Directory.Exists(path + "/thumbnails"))
            {
                Directory.CreateDirectory(Path.Combine(path, "thumbnails"));
            }
            if (!Directory.Exists(path + "/details"))
            {
                Directory.CreateDirectory(Path.Combine(path, "details"));
            }
           

            // THUMBNAIL
            using (var image = new MagickImage(Path.Combine(path, fileName)))
            {
                // Thumbnail
                int thumbWidth;
                int thumbHeight;
                
                if (image.Height > image.Width)
                {
                    // Thumbnail
                    thumbWidth = Convert.ToInt32(image.Width * ThumbnailSizeHeight / (double)image.Height);;
                    thumbHeight = ThumbnailSizeHeight;
                }
                else
                {
                    // Thumbnail
                    thumbWidth = ThumbnailSizeWidth;
                    thumbHeight = Convert.ToInt32(image.Height * ThumbnailSizeWidth / (double)image.Width);;
                }

                // Thumbnail
                var thumbPath = path + "/thumbnails";

                image.Resize(thumbWidth, thumbHeight);
                //image.Strip();
                image.Quality = 90;
                image.ColorSpace = ColorSpace.sRGB;
                image.Write(Path.Combine(thumbPath, fileName));
            }

            // DETAIL
            using (var image = new MagickImage(Path.Combine(path, fileName)))
            {
                int detailWidth;
                int detailHeight;
                if (image.Height > image.Width)
                {
                    // Detail
                    detailWidth = 0;
                    detailHeight = DetailSizeHeight;
                }
                else
                {
                    // Detail
                    detailWidth = DetailSizeWidth;
                    detailHeight = 0;
                }
                // If image is small than define size
                if (image.Height < DetailSizeHeight && image.Width < DetailSizeWidth)
                {
                    detailWidth = image.Width;
                    detailHeight = image.Height;
                }

                // Detail
                string detailPath = path + "/details";

                image.Resize(detailWidth, detailHeight);
                //image.Strip();
                image.Quality = 100;
                image.ColorSpace = ColorSpace.sRGB;
                image.Write(Path.Combine(detailPath, fileName));
            }
        }
    }
}