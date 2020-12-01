using EmployeesWebService.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeesWebService
{
    public class Startup
    {
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
            services.AddCors(options =>
           {
               options.AddDefaultPolicy (builder =>
               {
                   builder.AllowAnyOrigin ();
                   builder.AllowAnyMethod ();
                   builder.AllowAnyHeader ();
               });
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

            app.UseCors();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

