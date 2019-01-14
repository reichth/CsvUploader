using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CsvUploader.Data
{
    /// <summary>
    /// The data context of this application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebAppUser>()
                .HasKey(c => c.WebAppUserId);
        }


        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            optionsBuilder.UseSqlite(connection);
        }

        /// <summary>
        /// The set of web app user entities.
        /// </summary>
        public DbSet<WebAppUser> WebAppUsers { get; set; }

        /// <summary>
        /// The set of supplier entities.
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// The set of item entities.
        /// </summary>
        public DbSet<Item> Items { get; set; }

        /// <summary>
        /// Delivers login details for specified user credentials.
        /// </summary>
        /// <param name="userName">The user name credential.</param>
        /// <param name="password">The password credential.</param>
        /// <returns></returns>
        public WebAppUser GetLoginDetailsForUserCredentials(string userName, string password)
        {
            WebAppUser user = null;

            try
            {
                user = WebAppUsers.FirstOrDefault(u => u.LoginName.Equals(userName) && u.Password.Equals(password));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }

            return user;
        }
    }
}