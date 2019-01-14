using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvUploader.Data;
using CsvUploader.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsvUploader.Pages.Internal
{
    /// <summary>
    /// The page model class for upload page.
    /// </summary>
    [Authorize]
    public class UploadModel : PageModel
    {
        private readonly ApplicationDbContext dataContext;

        /// <summary>  
        /// Initializes a new instance of the <see cref="UploadModel"/> class.  
        /// </summary>  
        /// <param name="dataContext">Database manager context parameter</param>  
        public UploadModel(ApplicationDbContext dataContext)
        {
            this.dataContext = dataContext;
        }
        /// <summary>
        /// The list of file names to upload.
        /// </summary>
        /// <returns></returns>
        [BindProperty]
        [Display(Name = "Dateien hochladen")]
        public List<IFormFile> UploadFileName { get; set; }

        private int NewItems { get; set; }
        private int UpdatedItems { get; set; }

        /// <summary>  
        /// On Get method.  
        /// </summary>  
        public void OnGet()
        {
        }

        /// <summary>  
        /// POST: /Internal/Upload/Upload  
        /// </summary>  
        /// <returns>Return upload action</returns>
        [HttpPost]
        public async Task<IActionResult> OnPostUpload(IEnumerable<IFormFile> files)
        {
            try
            {
                foreach (var formFile in files)
                {
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "uploads",
                        FileNameFormatter.FormatWithTimeStamp(formFile.FileName));

                    var extension = Path.GetExtension(path);
                    if (!extension.ToLower().Equals(".csv"))
                    {
                        ViewData["FeedBack"] = "Falsches Dateiformat!";
                        break;
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var fileName = formFile.FileName.Replace(".csv", "");

                    LoadCsvFile(fileName, path);
                }
            }
            catch (Exception e)
            {
                if (string.IsNullOrEmpty(ViewData["FeedBack"].ToString())) ViewData["FeedBack"] = "Beim Hochladen ist ein Fehler aufgetreten.";
                Console.WriteLine(e);
            }

            if (NewItems>0)
                ViewData["NewItems"] = $"Es wurden {NewItems} neue Eintr‰ge hinzugef¸gt.";
            if (NewItems > 0)
                ViewData["UpdatedItems"] = $"Es wurden {UpdatedItems} Eintr‰ge aktualisiert.";

            return this.Page();
        }

        private void LoadCsvFile(string fileName, string filePath)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath, Encoding.Default).ToList();
                var firstLine = lines.FirstOrDefault();

                if (IsSupplierFile(firstLine))
                    ReadSupplierData(lines);

                if (IsItemFile(firstLine))
                    ReadItemData(fileName, lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool IsSupplierFile(string firstLine)
        {
            if (firstLine == null)
                return false;

            var expectedColumns = $"Lieferantennummer,Lieferantenname,Straﬂe,PLZ,Ort";

            return firstLine.Equals(expectedColumns);
        }

        private bool IsItemFile(string firstLine)
        {
            if (firstLine == null)
                return false;

            var expectedColums = $"Artikelnummer,Artikelbezeichnung";

            return firstLine.Equals(expectedColums);
        }

        private void ReadSupplierData(List<string> lines)
        {
            //first line contains no data.
            lines.Remove(lines.First());

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                
                AddOrUpdateSupplier(parts);
            }
        }

        private void AddOrUpdateSupplier(string[] parts)
        {
            if (parts.Length != 5)
                return;

            var existingSupplier = dataContext.Suppliers.FirstOrDefault(s => s.SupplierId.Equals(parts[0]));
            if (existingSupplier == null)
            {
                dataContext.Suppliers.Add(new Supplier
                {
                    SupplierId = parts[0],
                    Name = parts[1],
                    Street = parts[2],
                    ZipCode = parts[3],
                    City = parts[4]
                });
                dataContext.SaveChanges();
                NewItems++;
            }
            else
            {
                existingSupplier.Name = parts[1];
                existingSupplier.Street = parts[2];
                existingSupplier.ZipCode = parts[3];
                existingSupplier.City = parts[4];

                dataContext.SaveChanges();
                UpdatedItems++;
            }
        }

        private void ReadItemData(string supplierId, ICollection<string> lines)
        {
            //first line contains no data.
            lines.Remove(lines.First());

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 2)
                    continue;

                AddOrUpdateItems(supplierId, parts);
            }
        }

        private void AddOrUpdateItems(string supplierId, string[] parts)
        {
            if (string.IsNullOrEmpty(supplierId) || parts.Length != 2)
                return;

            var supplier = dataContext.Suppliers.FirstOrDefault(s => s.SupplierId.Equals(supplierId));
            if (supplier == null)
            {
                ViewData["FeedBack"] = "F¸r diese Artikel existiert noch kein Lieferant. Laden sie zuerst eine Lieferantenliste hoch.";
                throw new ArgumentException();
            }

            var existingSupplier = dataContext.Items.FirstOrDefault(s => s.SupplierId.Equals(supplierId) && s.ItemId.Equals(parts[0]));
            if (existingSupplier == null)
            {
                dataContext.Items.Add(new Item
                {
                    SupplierId = supplierId,
                    ItemId = parts[0],
                    Name = parts[1]
                });
                dataContext.SaveChanges();
                NewItems++;
            }
            else
            {
                existingSupplier.Name = parts[1];

                dataContext.SaveChanges();

                UpdatedItems++;
            }
        }
    }
}