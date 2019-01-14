using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using CsvUploader.Data;
using CsvUploader.Models;
using CsvUploader.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsvUploader.Pages.Internal
{
    /// <summary>  
    /// Dashboard page model class.  
    /// </summary>  
    [Authorize]
    public class DashboardModel : PageModel
    {
        public IOrderedEnumerable<UploadFileModel> UploadedFiles { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }

        private readonly ApplicationDbContext _dataContext;

        /// <summary>  
        /// Initializes a new instance of the <see cref="ShowSupplierModel"/> class.  
        /// </summary>  
        /// <param name="dataContext">Database manager context parameter</param>  
        public DashboardModel(ApplicationDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        /// <summary>
        /// The first name of user.
        /// </summary>
        /// <returns></returns>
        public string FirstName ()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GivenName));
            return claim==null? string.Empty :claim.Value;
        }

        /// <summary>
        /// The last name of user.
        /// </summary>
        /// <returns></returns>
        public string LastName()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname));
            return claim == null ? string.Empty : claim.Value;
        }

        /// <summary>  
        /// On Get method.  
        /// </summary>  
        public void OnGet(string sortOrder)
        {
            DateSort = string.IsNullOrEmpty(sortOrder) ? "date_desc" : string.Empty;
            NameSort = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            var filePaths = Directory.GetFiles(path, "*.csv",
                SearchOption.TopDirectoryOnly);

            var uploadedFiles = new List<UploadFileModel>();

            foreach (var filePath in filePaths)
            {
                var timeStamp = GetTimeStampFromFileName(filePath);

                if (timeStamp != null)
                    uploadedFiles.Add(new UploadFileModel
                    {
                        FileName = FileNameFormatter.ReFormatFromTimeStamp(Path.GetFileName(filePath)),
                        TimeStamp = (DateTime)timeStamp,
                        FullFileName = Path.GetFileName(filePath)
                    });
            }

            switch (sortOrder)
            {
                case "date_desc":
                    UploadedFiles = uploadedFiles.OrderByDescending(s => s.TimeStamp);
                    break;
                case "name":
                    UploadedFiles = uploadedFiles.OrderBy(s => s.FileName);
                    break;
                case "name_desc":
                    UploadedFiles = uploadedFiles.OrderByDescending(s => s.FileName);
                    break;
                default:
                    UploadedFiles = uploadedFiles.OrderBy(s => s.TimeStamp);
                    break;
            }
        }

        private DateTime? GetTimeStampFromFileName(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

            if (!fileNameWithoutExtension.Contains("_"))
                return null;

            var dateString = fileNameWithoutExtension.Substring(fileNameWithoutExtension.IndexOf('_')+1);
            var parsed = DateTime.TryParseExact(dateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var timeStamp);
            if (parsed)
                return timeStamp;

            return null;
        }

        /// <summary>  
        /// POST: /Internal/Dashboard/Upload  
        /// </summary>  
        /// <returns>Redirect to upload</returns>  
        public IActionResult OnPostUpload()
        {
            return RedirectToPage("/Internal/Upload");
        }

        /// <summary>  
        /// POST: /Internal/Dashboard/Upload  
        /// </summary>  
        /// <returns>Redirect to show supplier</returns>  
        public IActionResult OnPostShowSupplier()
        {
            return RedirectToPage("/Internal/ShowSupplier");
        }
    }
}