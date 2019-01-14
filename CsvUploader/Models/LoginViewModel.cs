using System.ComponentModel.DataAnnotations;

namespace CsvUploader.Models
{
    /// <summary>  
    /// Login view model class.  
    /// </summary>  
    public class LoginViewModel
    {
        /// <summary>  
        /// Gets or sets to username address.  
        /// </summary>  
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>  
        /// Gets or sets to password address.  
        /// </summary>  
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}