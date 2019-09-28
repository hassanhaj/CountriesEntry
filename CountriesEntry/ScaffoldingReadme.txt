        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            app.Use(async (context, next) =>
             {
                  logger.LogInformation("Request Started at: " + DateTime.Now.ToString("HH:mm:ss"));
                 //await e.Response.WriteAsync("Hello");
                 await next.Invoke();
                 logger.LogInformation("Request Finished at: " + DateTime.Now.ToString("HH:mm:ss"));
             });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello");
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRequestInfoMiddleware();
            //app.UseDirectoryBrowser();
            // app.UseDefaultFiles(new DefaultFilesOptions {
            //     DefaultFileNames = new[] { "js/site.js" }
            // });
            //  app.UseFileServer();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
