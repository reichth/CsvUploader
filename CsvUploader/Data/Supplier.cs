namespace CsvUploader.Data
{
    /// <summary>
    /// The entity model for auppliers (Lieferant).
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// The supplier identifer.
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
        /// The zipcode of supplier.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// The city of supplier.
        /// </summary>
        public string City { get; set; }
    }
}