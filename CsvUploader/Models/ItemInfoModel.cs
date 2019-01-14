namespace CsvUploader.Models
{
    /// <summary>
    /// Data model for list of suppliers.
    /// </summary>
    public class ItemInfoModel
    {
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