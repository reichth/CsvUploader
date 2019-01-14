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
    /// The page model for show items page.
    /// </summary>
    [Authorize]
    public class ShowItemsModel : PageModel
    {
        /// <summary>
        /// The queryable items of supplier. 
        /// </summary>
        public IQueryable<ItemInfoModel> Items { get; set; }

        /// <summary>
        /// The supplier identifier for items.
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        /// The sort parameter for identifier of item.
        /// </summary>
        public string IdSort { get; set; }

        /// <summary>
        /// The sort parameter for name of item.
        /// </summary>
        public string NameSort { get; set; }

        private readonly ApplicationDbContext dataContext;

        /// <summary>  
        /// Initializes a new instance of the <see cref="ShowItemsModel"/> class.  
        /// </summary>  
        /// <param name="dataContext">Database manager context parameter</param>  
        public ShowItemsModel(ApplicationDbContext dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <summary>  
        /// On Get method.  
        /// </summary>  
        public async Task OnGetAsync(string supplierId, string sortOrder)
        {
            if (string.IsNullOrEmpty(supplierId))
                RedirectToPage("/Internal/Dashboard");

            SupplierId = supplierId;

            IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : string.Empty;
            NameSort = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            var supplierListItems = dataContext.Items.Where(i => i.SupplierId.Equals(supplierId)).Select(s =>
                new ItemInfoModel
                {
                    ItemId = s.ItemId,
                    Name = s.Name
                });


            switch (sortOrder)
            {
                case "id_desc":
                    Items = supplierListItems.OrderByDescending(s => s.ItemId);
                    break;
                case "name":
                    Items = supplierListItems.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    Items = supplierListItems.OrderByDescending(s => s.Name);
                    break;
                default:
                    Items = supplierListItems.OrderBy(s => s.ItemId);
                    break;
            }

            await Items.AsNoTracking().ToListAsync();
        }
    }


}