using System;
using System.IO;

namespace CsvUploader.Utility
{
    /// <summary>
    /// helper class to format/reformat uploaded filenames.
    /// </summary>
    public class FileNameFormatter
    {
        /// <summary>
        /// Delivers formatted file name with timestamp included.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FormatWithTimeStamp(string fileName)
        {
            var now = DateTime.UtcNow;
            var extension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            return string.Concat(fileNameWithoutExtension, "_", now.ToString("yyyyMMddHHmmss"), extension);
        }

        /// <summary>
        /// Delivers reformatted file name from name with timestamp.
        /// </summary>
        /// <param name="fileNameWithTimeStamp"></param>
        /// <returns></returns>
        public static string ReFormatFromTimeStamp(string fileNameWithTimeStamp)
        {
            var extension = Path.GetExtension(fileNameWithTimeStamp);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithTimeStamp);

            var fileName = fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.LastIndexOf('_'));

            return string.Concat(fileName, extension);
        }
    }
}
