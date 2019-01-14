namespace CsvUploader.Models
{
    /// <summary>
    /// Data model for list of suppliers.
    /// </summary>
    public class SupplierInfoModel
    {
        /// <summary>
        /// The supplier identifier.
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        /// The name of supplier.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The street of supplier.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// The zip code of supplier.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// The city of supplier.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The count of items of supplier.
        /// </summary>
        public int ItemCount { get; set; }
    }
}