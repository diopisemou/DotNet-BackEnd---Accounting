using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Helpers.CustomAttribute;
using BackEnd.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      services.AddCors(options => {
        options.AddPolicy(MyAllowSpecificOrigins,
        builder => {
          builder.WithOrigins("http://localhost:3000",
                                      "http://localhost:63012").AllowAnyHeader().AllowAnyMethod();
        });
      });
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);
      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = false,
              ValidateIssuerSigningKey = true,
              ValidIssuer = appSettings.JwtIssuer,
              ValidAudience = appSettings.JwtIssuer,
              IssuerSigningKey = securityKey
            };
          });

      // configure DI for application services
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IEventService, EventUserLog>();
      services.AddScoped<HasPermission>();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      services.AddDbContext<MyDatabaseContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("MyDatabase")));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        app.UseHsts();
      }
      app.UseCors(MyAllowSpecificOrigins);
      app.UseStaticFiles();
      app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
