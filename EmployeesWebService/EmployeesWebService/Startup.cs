using System;
using EmployeesWebService.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
    
namespace EmployeesWebService
{
    public class Startup
    {
        public const string MY_CORS = "AllowSpecificOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {

            services.AddControllers ();
            services.AddMemoryCache ();
            services.AddCors (options =>
           {

               options.AddPolicy (MY_CORS, builder =>
              {
                  builder
                    .WithOrigins ("http://localhost:3000")
                    .WithMethods ("GET", "POST", "PUT", "DELETE")
                    .WithHeaders ("accept", "content-type", "origin", "x-custom-header", "authorization");

                  //builder.WithOrigins ("http://localhost:3000");
                  //builder.AllowAnyMethod();
                  //builder.AllowAnyHeader();
              });            

           });

            services.AddDataProtection ()
                .SetApplicationName("MyApp")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(14))
                //.PersistKeysToFileSystem (new System.IO.DirectoryInfo (@"\\server\share\")) %APPDATA%
                //.ProtectKeysWithDpapi()
                .UseCryptographicAlgorithms (new AuthenticatedEncryptorConfiguration ()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            // habilitando JWT Authentication
            var jwtSection = Configuration.GetSection ("JwtSettings");
            services.Configure<JWTSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JWTSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes (jwtSettings.Secret);

            services.AddAuthentication(x =>
           {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           }).AddJwtBearer(y =>
           {
               y.RequireHttpsMetadata = false;
               y.SaveToken = true;
               y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey (key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });


            services.AddScoped<IUserService, UserService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting ();

            app.UseCors (MY_CORS); // para habiitar cors, colocar aquí 

            app.UseAuthentication ();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) =>
           {
               //context.Response.Headers.Add ("Content-Security-Policy", "default-src 'self'; report-uri /cspreport");
               context.Response.Headers.Add ("x-xss-protection", "1");
               await next ();
           });
        }
    }
}

