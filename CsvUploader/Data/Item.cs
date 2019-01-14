namespace CsvUploader.Data
{
    /// <summary>
    /// Entity model for items (Artikel).
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The supplier identifier.
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        /// The item identifier.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// The name of item.
        /// </summary>
        public string Name { get; set; }
    }
}