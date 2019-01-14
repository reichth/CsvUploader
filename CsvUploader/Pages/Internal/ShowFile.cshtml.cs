using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvUploader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsvUploader.Pages.Internal
{
    /// <summary>
    /// The page model class for show file view.
    /// </summary>
    [Authorize]
    public class ShowFileModel : PageModel
    {
        public IOrderedEnumerable<SupplierInfoModel> Suppliers { get; set; }
        /// <summary>
        /// The queryable items of supplier. 
        /// </summary>
        public IOrderedEnumerable<ItemInfoModel> Items { get; set; }

        /// <summary>
        /// The full name of uploaded file.
        /// </summary>
        public string FullFileName { get; set; }

        /// <summary>
        /// The supplier identifier for items.
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        /// The sort parameter for identifier of item.
        /// </summary>
        public string IdSort { get; set; }

        /// <summary>
        /// The sort parameter for name.
        /// </summary>
        public string NameSort { get; set; }

        /// <summary>
        /// boolean, if specified file is supplier file.
        /// </summary>
        public bool IsSupplierFile;
        /// <summary>
        /// The Onget method.
        /// </summary>
        public void OnGet(string fileName, string sortOrder)
        {
            FullFileName = fileName;

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            try
            {
                LoadCsvFile(fileName, path, sortOrder);
            }
            catch (Exception e)
            {
                ViewData["Feedback"] = "Beim Laden der Datei ist ein Fehler aufgetreten.";
                Console.WriteLine(e);
            }
        }

        private void LoadCsvFile(string fileName, string filePath, string sortOrder)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath, Encoding.Default).ToList();
                var firstLine = lines.FirstOrDefault();

                if (CheckIfIsSupplierFile(firstLine))
                {
                    IsSupplierFile = true;
                    var supplierList = ReadSupplierData(lines);
                    SetOrderedSuppliers(supplierList, sortOrder);
                }

                if (CheckIfIsItemFile(firstLine))
                {
                    var itemList = ReadItemData(fileName, lines);
                    SetOrderedItems(itemList, sortOrder);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SetOrderedItems(IEnumerable<ItemInfoModel> itemList, string sortOrder)
        {
            IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : string.Empty;
            NameSort = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            switch (sortOrder)
            {
                case "id_desc":
                    Items = itemList.OrderByDescending(s => s.ItemId);
                    break;
                case "name":
                    Items = itemList.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    Items = itemList.OrderByDescending(s => s.Name);
                    break;
                default:
                    Items = itemList.OrderBy(s => s.ItemId);
                    break;
            }
        }

        private void SetOrderedSuppliers(IEnumerable<SupplierInfoModel> supplierList, string sortOrder)
        {
            IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : string.Empty;
            NameSort = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            switch (sortOrder)
            {
                case "id_desc":
                    Suppliers = supplierList.OrderByDescending(s => s.SupplierId);
                    break;
                case "name":
                    Suppliers = supplierList.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    Suppliers = supplierList.OrderByDescending(s => s.Name);
                    break;
                default:
                    Suppliers = supplierList.OrderBy(s => s.SupplierId);
                    break;
            }
        }

        private bool CheckIfIsSupplierFile(string firstLine)
        {
            if (firstLine == null)
                return false;

            var expectedColumns = $"Lieferantennummer,Lieferantenname,Straﬂe,PLZ,Ort";

            return firstLine.Equals(expectedColumns);
        }

        private bool CheckIfIsItemFile(string firstLine)
        {
            if (firstLine == null)
                return false;

            var expectedColums = $"Artikelnummer,Artikelbezeichnung";

            return firstLine.Equals(expectedColums);
        }
        private IEnumerable<SupplierInfoModel> ReadSupplierData(List<string> lines)
        {
            var supplierList = new List<SupplierInfoModel>();

            //first line contains no data.
            lines.Remove(lines.First());

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                supplierList.Add(ReadSupplier(parts));
            }

            return supplierList;
        }

        private SupplierInfoModel ReadSupplier(string[] parts)
        {
            if (parts.Length != 5)
                throw new ArgumentException();

            return new SupplierInfoModel
            {
                SupplierId = parts[0],
                Name = parts[1],
                Street = parts[2],
                ZipCode = parts[3],
                City = parts[4]
            };
        }

        private IEnumerable<ItemInfoModel> ReadItemData(string supplierId, ICollection<string> lines)
        {
            var itemList = new List<ItemInfoModel>();
            //first line contains no data.
            lines.Remove(lines.First());

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 2)
                    continue;

                itemList.Add(ReadItem(parts));
            }

            return itemList;
        }

        private ItemInfoModel ReadItem(string[] parts)
        {
            if (parts.Length != 2)
                throw new ArgumentException();

            return new ItemInfoModel()
            {
                ItemId = parts[0],
                Name = parts[1]
            };
        }
    }
}