using System;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace CMS.Web.Utilities;

public static class ImageHelpers
{
    private const int DetailSizeWidth = 2048; // width
    private const int DetailSizeHeight = 1536; // height

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
            
        const int thumbnailQuality = 75;
        const int detailQuality = 90;
            
        var thumbnailImageFormat = SKEncodedImageFormat.Webp;
        var detailImageFormat = SKEncodedImageFormat.Webp;
        var exportFileName = fileName.Split('.').First();
            
        using var input = File.OpenRead(Path.Combine(path, fileName));
        using var inputStream = new SKManagedStream(input);
        using var imgStream = new SKManagedStream(input);
        using var skData = SKData.Create(input);
        using var codec = SKCodec.Create(skData);
        using var originalBitmap = SKBitmap.Decode(codec);
        var original = AutoOrient(originalBitmap, codec.EncodedOrigin);
            
        /*
         * THUMBNAIL
         */
        int thumbWidth;
        int thumbHeight;
            
        if (original.Height > original.Width)
        {
            // Thumbnail
            thumbWidth = Convert.ToInt32(original.Width * ThumbnailSizeHeight / (double)original.Height);
            thumbHeight = ThumbnailSizeHeight;
        }
        else
        {
            // Thumbnail
            thumbWidth = ThumbnailSizeWidth;
            thumbHeight = Convert.ToInt32(original.Height * ThumbnailSizeWidth / (double)original.Width);
        }
            
        // Thumbnail
        var thumbPath = Path.Combine(path, "thumbnails");
        using var resizedToThumb = original.Resize(new SKImageInfo(thumbWidth, thumbHeight), SKFilterQuality.Medium);
        if (resizedToThumb == null) return;
            
        using var thumbImage = SKImage.FromBitmap(resizedToThumb);
        using var output = File.OpenWrite(Path.Combine(thumbPath, exportFileName + "." + thumbnailImageFormat.ToString().ToLower()));
        thumbImage.Encode(thumbnailImageFormat, thumbnailQuality).SaveTo(output);
            
        /*
         * DETAIL
         */
        int detailWidth;
        int detailHeight;
        if (original.Height > original.Width)
        {
            // Detail
            detailWidth = Convert.ToInt32(original.Width * DetailSizeHeight / (double)original.Height);
            detailHeight = DetailSizeHeight;
        }
        else
        {
            // Detail
            detailWidth = DetailSizeWidth;
            detailHeight = Convert.ToInt32(original.Height * DetailSizeWidth / (double)original.Width);
        }
        // If image is small than define size
        if (original.Height < DetailSizeHeight && original.Width < DetailSizeWidth)
        {
            detailWidth = original.Width;
            detailHeight = original.Height;
        }
            
        // Detail
        var detailPath = Path.Combine(path, "details");
        using var resizedToDetail = original.Resize(new SKImageInfo(detailWidth, detailHeight), SKFilterQuality.High);
        if (resizedToDetail == null) return;
            
        using var detailImage = SKImage.FromBitmap(resizedToDetail);
        using var detailOutput = File.OpenWrite(Path.Combine(detailPath, exportFileName + "." + detailImageFormat.ToString().ToLower()));
        detailImage.Encode(detailImageFormat, detailQuality).SaveTo(detailOutput);
    }
        
    private static SKBitmap AutoOrient(SKBitmap bitmap, SKEncodedOrigin origin)
    {
        SKBitmap rotated;
        switch (origin)
        {
            case SKEncodedOrigin.BottomRight:
                using (var surface = new SKCanvas(bitmap))
                {
                    surface.RotateDegrees(180, bitmap.Width / 2, bitmap.Height / 2);
                    surface.DrawBitmap(bitmap.Copy(), 0, 0);
                }
                return bitmap;
            case SKEncodedOrigin.RightTop:
                rotated = new SKBitmap(bitmap.Height, bitmap.Width);
                using (var surface = new SKCanvas(rotated))
                {
                    surface.Translate(rotated.Width, 0);
                    surface.RotateDegrees(90);
                    surface.DrawBitmap(bitmap, 0, 0);
                }
                return rotated;
            case SKEncodedOrigin.LeftBottom:
                rotated = new SKBitmap(bitmap.Height, bitmap.Width);
                using (var surface = new SKCanvas(rotated))
                {
                    surface.Translate(0, rotated.Height);
                    surface.RotateDegrees(270);
                    surface.DrawBitmap(bitmap, 0, 0);
                }
                return rotated;
            case SKEncodedOrigin.TopLeft:
            case SKEncodedOrigin.TopRight:
            case SKEncodedOrigin.BottomLeft:
            case SKEncodedOrigin.LeftTop:
            case SKEncodedOrigin.RightBottom:
            default:
                return bitmap;
        }
    }
}