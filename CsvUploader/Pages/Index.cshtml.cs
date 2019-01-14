using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CsvUploader.Data;
using CsvUploader.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsvUploader.Pages
{
    /// <summary>  
    /// Index page model class.  
    /// </summary>  
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dataContext; 

        /// <summary>  
        /// Initializes a new instance of the <see cref="IndexModel"/> class.  
        /// </summary>  
        /// <param name="dataContext">Database manager context parameter</param>  
        public IndexModel(ApplicationDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        #region Public Properties  

        /// <summary>  
        /// Gets or sets login model property.  
        /// </summary>  
        [BindProperty]
        public LoginViewModel LoginModel { get; set; }

        #endregion

        #region On Get method.  

        /// <summary>  
        /// GET: /Index  
        /// </summary>  
        /// <returns>Returns - Appropriate page </returns>  
        public IActionResult OnGet()
        {
            try
            {
                // Verification.  
                if (this.User.Identity.IsAuthenticated)
                {
                    // Home Page.  
                    return this.RedirectToPage("/Internal/Dashboard");
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.Page();
        }

        #endregion

        #region On Post Login method.  

        /// <summary>  
        /// POST: /Index/LogIn  
        /// </summary>  
        /// <returns>Returns - Appropriate page </returns>  
        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                // Verification.  
                if (ModelState.IsValid)
                {
                    // Initialization.  
                    var loginDetails = _dataContext.GetLoginDetailsForUserCredentials(this.LoginModel.Username, this.LoginModel.Password);

                    // Verification.  
                    if (loginDetails != null)
                    {
                        // Login In.  
                        await this.SignInUser(loginDetails.LoginName, loginDetails.FirstName, loginDetails.LastName,
                            loginDetails.WebAppUserId,
                            false);

                        // Info.  
                        return this.RedirectToPage("/Internal/Dashboard");
                    }
                    else
                    {
                        // Setting.  
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            ViewData["FeedBack"] = "Beim Anmelden ist ein Fehler aufgetreten.";

            return this.Page();
        }

        #endregion

        #region Helpers  

        #region Sign In method.  

        /// <summary>  
        /// Sign In User method.  
        /// </summary>
        /// <param name="loginName">FirstName parameter.</param>
        /// <param name="firstName">FirstName parameter.</param>
        /// <param name="lastName">LastName parameter.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isPersistent">Is persistent parameter.</param>  
        /// <returns>Returns - await task</returns>  
        private async Task SignInUser(string loginName, string firstName, string lastName, int userId, bool isPersistent)
        {
            // Initialization.  
            var claims = new List<Claim>();

            try
            {
                // Setting  
                claims.Add(new Claim(ClaimTypes.Name, loginName));
                claims.Add(new Claim(ClaimTypes.GivenName, firstName));
                claims.Add(new Claim(ClaimTypes.Surname, lastName));
                claims.Add(new Claim("userid", userId.ToString()));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.  
                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}