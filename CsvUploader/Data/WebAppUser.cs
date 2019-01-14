using Microsoft.AspNetCore.Identity;

namespace CsvUploader.Data
{
    /// <summary>
    /// The additional profile data for application users.
    /// </summary>
    public class WebAppUser : IdentityUser
    {
        /// <summary>
        /// The web app user id.
        /// </summary>
        public int WebAppUserId { get; set; }

        /// <summary>
        /// The first name of user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name iof user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// ´The login name of user.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// The password of user.
        /// </summary>
        public string Password { get; set; }
    }
}