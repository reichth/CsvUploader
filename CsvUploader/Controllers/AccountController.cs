using System.Linq;
using CsvUploader.Data;
using CsvUploader.Models;
using Microsoft.AspNetCore.Mvc;

namespace CsvUploader.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController
    {
        private readonly ApplicationDbContext dataContext;

        /// <summary>  
        /// Initializes a new instance of the <see cref="AccountController"/> class.  
        /// </summary>  
        /// <param name="dataContext">Database manager context parameter</param>  
        public AccountController(ApplicationDbContext dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <summary>
        /// Endpoint to create new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostUser([FromBody] UserPostModel model)
        {
            if (model == null ||
                string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.LastName) ||
                string.IsNullOrEmpty(model.LoginName) ||
                string.IsNullOrEmpty(model.Password))
                return new BadRequestObjectResult("invalid data");

            var existing = dataContext.WebAppUsers.FirstOrDefault(u => u.LoginName.Equals(model.LoginName));

            if (existing != null)
                return new BadRequestObjectResult("user already existing");

            var newUser = new WebAppUser
            {
                LoginName = model.LoginName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password
            };
            dataContext.WebAppUsers.Add(newUser);
            dataContext.SaveChanges();

            return new OkObjectResult(newUser);
        }
    }
}
