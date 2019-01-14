using System;

namespace CsvUploader.Models
{
    /// <summary>
    /// Data model for uploaded file.
    /// </summary>
    public class UploadFileModel
    {
        /// <summary>
        /// The supplier identifier.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The name of supplier.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The fullname of file in uploads folder.
        /// </summary>
        public string FullFileName { get; set; }
    }
}