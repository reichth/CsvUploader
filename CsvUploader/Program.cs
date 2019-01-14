using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CsvUploader
{
    /// <summary>
    /// The application start class.
    /// </summary>
    public class Program
    {
        /// <summary>  
        /// The main function of application  
        /// </summary>  
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
