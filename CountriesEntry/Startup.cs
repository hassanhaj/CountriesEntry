using CountriesEntry.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CountriesEntry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Auth: Configure Cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.Cookie.Expiration = TimeSpan.FromMinutes(35);
            });


            services.AddMvc(options=> {
                options.Filters.Add(typeof(ValidateCountry));
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add(typeof(CacheResourceFilter));
            })
          //      options.Filters.Add(typeof(CustomExceptionFilter));
              //      options.Filters.Add(typeof(ValidationActionFilter))         // By type
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    // Auth: Razor Pages
                    options.Conventions.AuthorizeFolder("/Identity/Account/Manage");
                    options.Conventions.AuthorizeFolder("/Identity/Account/Logout");
                })
                                .AddJsonOptions(o =>
                                {
                                    o.SerializerSettings.ReferenceLoopHandling =
                                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                                    o.SerializerSettings.MaxDepth = 1;
                                });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<RequestInfo>();
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogWarning("aaa {name}", new[] { "myName" });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home");
            }

          //  app.Run((context) => context.Response.WriteAsync("aa"));

            app.Use(async (context, next) =>
            {
                logger.LogInformation("Request Started at: " + DateTime.Now.ToString("HH:mm:ss"));
                await next.Invoke();
                logger.LogInformation("Request Finished at: " + DateTime.Now.ToString("HH:mm:ss"));
            });
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRequestInfoMiddleware();

            app.UseStaticFiles();
            //app.UseFileServer();
            //app.UseDirectoryBrowser();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
