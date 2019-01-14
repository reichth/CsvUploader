using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsvUploader.Pages.Internal
{
    /// <summary>  
    /// Logout page model class.  
    /// </summary>  
    [Authorize]
    public class LogoutModel : PageModel
    {
        /// <summary>  
        /// On Get method.  
        /// </summary>  
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Setting.  
                    var authenticationManager = Request.HttpContext;

                    // Sign Out. 

                    await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return RedirectToPage("/Index");
        }
    }
}