using System.ComponentModel.DataAnnotations;

namespace CsvUploader.Models
{
    /// <summary>
    /// The post model to create new web app user in database.
    /// </summary>
    public class UserPostModel
    {
        /// <summary>
        /// The first name of user.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of user.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The login name of user.
        /// </summary>
        [Required]
        public string LoginName { get; set; }

        /// <summary>
        /// The password of user.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
