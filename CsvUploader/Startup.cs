using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using CsvUploader.Data;

namespace CsvUploader
{
    /// <summary>
    /// The startup class of application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The startup configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>  
        /// Initializes a new instance of the <see cref="Startup"/> class.  
        /// </summary>  
        /// <param name="configuration">The startup configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");


            using (var client = new ApplicationDbContext())
            {
                client.Database.EnsureCreated();
                client.Database.Migrate();

                if (false == client.WebAppUsers.Any())
                    client.WebAppUsers.Add(new WebAppUser
                    {
                        FirstName = "Max",
                        LastName = "Mustermann",
                        LoginName = "MaxMustermann",
                        Password = "12345"
                    });
                client.SaveChanges();
            }
        }

        /// <summary>
        ///  This method is used to add services to the container.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Index");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
            });
            
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(
                c =>
                {
                    var applicationVersion = PlatformServices.Default.Application.ApplicationVersion;

                    c.SwaggerDoc(
                        "v0",
                        new Info
                        {
                            Title = $"Web Service : WebApplication1 V {applicationVersion} ",
                            Version = "v0",
                            Description = "REST Interface to this WebApplication.",
                            Contact = new Contact { Name = "Thomas Reich", Email = "reichth@freenet.de" }
                        });

                    // Set the comments path for the Swagger JSON and UI.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var xmlPath = Path.Combine(basePath, "CsvUploader.xml");
                    c.IncludeXmlComments(xmlPath);

                    c.EnableAnnotations();
                });
            services.AddMvc().AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/");
                    options.Conventions.AllowAnonymousToPage("/Index");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="environment">The hosting environment.</param>
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment environment)
        {
            applicationBuilder.UseAuthentication();

            applicationBuilder.UseExceptionHandler("/Error");
            applicationBuilder.UseHsts();

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseCookiePolicy();

            // Todo Disable in production
            applicationBuilder.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            applicationBuilder.UseSwaggerUI(c => { c.SwaggerEndpoint("v0/swagger.json", "API V0"); });

            applicationBuilder.UseMvc();
        }
    }
}
