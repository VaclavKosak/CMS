using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace CMS.Web.Utilities;

public class DateComparator : IComparer
{
    public int Compare(object x, object y)
    {
        if (x is not string s1) return 0;

        if (y is not string s2) return 0;

        var objX = GetImageDate(s1);
        var objY = GetImageDate(s2);

        return DateTime.Compare(objX, objY);
    }

    private static DateTime GetImageDate(string name)
    {
        var directories = ImageMetadataReader.ReadMetadata(name);
        var exifSubDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        var originalDate = exifSubDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
        var date = DateTime.TryParseExact(originalDate, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var tempDate)
            ? tempDate
            : DateTime.Now;
        return date;
    }
}