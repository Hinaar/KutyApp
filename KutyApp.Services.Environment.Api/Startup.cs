using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using KutyApp.Services.Environment.Api.DI;
using KutyApp.Services.Environment.Api.Extensions;
using KutyApp.Services.Environment.Api.Filter;
using KutyApp.Services.Environment.Bll.Configuration;
using KutyApp.Services.Environment.Bll.DI;
using KutyApp.Services.Environment.Bll.Entities;
using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace KutyApp.Services.Environment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString(nameof(KutyAppServiceDbContext));

            services.AddDbContext<KutyAppServiceDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    b => {
                        b.UseNetTopologySuite();
                        b.MigrationsAssembly($"{nameof(KutyApp)}.{nameof(Services)}.{nameof(Environment)}.{nameof(Api)}"); }
                    );
            });

            services.AddDefaultIdentity<User>(
                o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequiredLength = 6;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireLowercase = false;

                    o.User.RequireUniqueEmail = true;
                    o.SignIn.RequireConfirmedEmail = false;
                }
            ).AddEntityFrameworkStores<KutyAppServiceDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddBllManagers();
            services.AddApiManagers();
            services.AddContext();

            //configok
            services.Configure<JwtSettings>(Configuration.GetSection(nameof(JwtSettings)));

            //Filtereket majd ide
            //---

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = GetTokenValidationParameters();
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.IncludeErrorDetails = true;
            });

            services.AddMvc(o =>
            {
                o.Filters.Add<KutyAppContextFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapper();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "KutyApp Service Api", Version = "v1" });
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //Middlewareket majd ide
            //---

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            string publicKey = $"{nameof(JwtSettings)}:{nameof(JwtSettings.PublicKey)}";
            string issuerKey = $"{nameof(JwtSettings)}:{nameof(JwtSettings.Issuer)}";
            string audienceKey = $"{nameof(JwtSettings)}:{nameof(JwtSettings.Audience)}";

            RsaSecurityKey signingKey;
           
            RSA publicRsa = RSA.Create();
            publicRsa.LoadFromXmlString(Configuration.GetSection(publicKey).Value);
            signingKey = new RsaSecurityKey(publicRsa);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection(issuerKey).Value,
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection(audienceKey).Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return tokenValidationParameters;
        }
    }
}
