using System.Linq;
using System.Threading.Tasks;
using CsvUploader.Data;
using CsvUploader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CsvUploader.Pages.Internal
{
    /// <summary>
    /// The page model for show supplier page.
    /// </summary>
    [Authorize]
    public class ShowSupplierModel : PageModel
    {
        public IQueryable<SupplierInfoModel> Suppliers { get; set; }
        public string NameSort { get; set; }
        public string IdSort { get; set; }

        private readonly ApplicationDbContext _dataContext;

        /// <summary>  
        /// Initializes a new instance of the <see cref="ShowSupplierModel"/> class.  
        /// </summary>  
        /// <param name="dataContext">Database manager context parameter</param>  
        public ShowSupplierModel(ApplicationDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        /// <summary>  
        /// On Get method.  
        /// </summary>  
        public async Task OnGetAsync(string sortOrder)
        {
            IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : string.Empty;
            NameSort = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            var supplierListItems = _dataContext.Suppliers.Select(s => new SupplierInfoModel
            {
                SupplierId = s.SupplierId,
                Name = s.Name,
                ItemCount = _dataContext.Items.Count(i => i.SupplierId.Equals(s.SupplierId))
            });

            
            switch (sortOrder)
            {
                case "id_desc":
                    Suppliers = supplierListItems.OrderByDescending(s => s.SupplierId);
                    break;
                case "name":
                    Suppliers = supplierListItems.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    Suppliers = supplierListItems.OrderByDescending(s => s.Name);
                    break;
                default:
                    Suppliers = supplierListItems.OrderBy(s => s.SupplierId);
                    break;
            }

            await Suppliers.AsNoTracking().ToListAsync();
        }
    }

    
}