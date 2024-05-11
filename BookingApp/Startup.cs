using System;
using System.IO;
using System.Text;
using BookingApp.Service;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingApp
{
    public class Startup
    {
        public const string ADDRESS = "https://localhost:5001/";
        public const string JWT_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        private const string DB_FIRST_TIME = "dbFirstTime.txt";
        private string PushExpression = "*/10 * * * *";
        private string DeleteNotifyExpression = "* * */1 * *";
        private string TimeZone = "SE Asia Standard Time";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            if (!System.IO.File.Exists(DB_FIRST_TIME))
            {
                try 
                {
                    using (FileStream fs = File.Create(DB_FIRST_TIME)) { }
                }
                catch { }

                using var db = new DB.Classes.DB.AppDbContext();
                System.IO.File.SetAttributes(DB_FIRST_TIME, System.IO.File.GetAttributes(DB_FIRST_TIME) | System.IO.FileAttributes.Hidden);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllersWithViews();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddScoped<ISchedulerService, SchedulerService>();

            //Provide a secret key to Encrypt and Decrypt the Token
            var SecretKey = Encoding.ASCII.GetBytes(JWT_KEY);

            //Configure JWT Token Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //Same Secret key will be used while creating the token
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                            ValidateIssuer = true,
                    //Usually, this is your application base URL
                    ValidIssuer = ADDRESS,
                            ValidateAudience = true,
                    //Here, we are creating and using JWT within the same application.
                    //In this case, base URL is fine.
                    //If the JWT is created using a web service, then this would be the consumer URL.
                    ValidAudience = ADDRESS,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            var hangfireSetting = Configuration
                      .GetSection("Hangfire")
                      .Get<HangfireSetting>();
            services.AddSingleton(hangfireSetting);
            ///add AddHangfire cron job
            services.AddHangfire(configuration => configuration
                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseMemoryStorage());
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                                                        //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                    .AllowCredentials()); // allow credentials
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {

                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"file")),
                RequestPath = new PathString("/file")
            });
            app.UseRouting();

            app.UseAuthorization();

            //app.UseCookiePolicy();
            app.UseSession();

            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            //Add JWToken Authentication service
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            ///add hangfire
            //app.UseHangfireDashboard("/cgis/hangfire");
            //GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });
            //var options = new BackgroundJobServerOptions
            //{
            //    ServerName = string.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString()),
            //    WorkerCount = 1,
            //    ServerTimeout = TimeSpan.FromMinutes(120)
            //};
            //RecurringJob.AddOrUpdate<ISchedulerService>(ms => ms.AutoTrecking(), PushExpression, TimeZoneInfo.FindSystemTimeZoneById(TimeZone));
            //_ = app.UseHangfireServer(options);

        }
    }
}