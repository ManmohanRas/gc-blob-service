using System;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Reflection;
//using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using morris_azstorage_service.Helpers;
using Microsoft.AspNetCore.StaticFiles;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.AccessTokenValidation;

namespace morris_azstorage_service
{
  public class Startup
    {
        readonly string MyCORSPolicy = "_myCORSPolicy";
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}
            //                           ).AddJwtBearer(o =>
            //                           {
            //                               o.Authority = Configuration["MorrisSTSEndpoint"];
            //                               o.Audience = Configuration["APIResource"];
            //                               // IdentityServer emits a typ header by default, recommended extra check
            //                               //o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            //                               //o.RequireHttpsMetadata = false;

            //                               o.TokenValidationParameters = new TokenValidationParameters
            //                               { ValidateAudience = false };
            //                           });

          


            //// adds an authorization policy to make sure the token is for scope 'api1'
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiScope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", "morris_storage_api");
            //    });
            //    options.AddPolicy("mcprimaAdmin", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireRole("morrisdeveloper_admin","sysadmin");
            //    });
           
            //});


            string[] hsts = Configuration.GetSection("CORSConfiguration:AllowedHosts").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyCORSPolicy,
                                  builder =>
                                  {
                                      builder.WithOrigins(hsts)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                                  });
            });


            //  services.Configure<AzureStorageBlobOptions>(Configuration.GetSection("AzureStorageBlobOptions"));

            var connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];
            // Set up CORS policy to allow external websites to access the service
         
   
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MorrisGIS AzBlob API",
                    Description = "Morris County GIS Azure Blob Storage Service",
                    TermsOfService = new Uri("https://morrisgisapps.co.morris.nj.us/termsofservice"),
                    Contact = new OpenApiContact
                    {
                        Name = "Morris County GIS",
                        Email = "mcgis@co.morris.nj.us",
                        Url = new Uri("https://morrisgisapps.co.morris.nj.us"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.OperationFilter<AddAuthHeaderOperationFilter>();
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
   
                //    services.AddSingleton();


                // Add repositories
                //   services.AddSingleton<IBlobStorageRepository, BlobStorageRepository>();

            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
     
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", 
                        optional: false, 
                        reloadOnChange: true)
            .AddEnvironmentVariables("AzureStorageBlobOptions_")
            .Build();
            
        

            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors(MyCORSPolicy);
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
                {
                c.RoutePrefix = "v1";
       var basePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
       c.SwaggerEndpoint($"{basePath}/swagger/{c.RoutePrefix}/swagger.json", "Morris Azure Blob Storage API");
               
            });
         //   app.UseCookiePolicy(); // Before UseAuthentication or anything else that writes cookies.
            
            app.UseRouting();
            app.UseAuthentication();
           app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

           public static bool DisallowsSameSiteNone(string userAgent)
                {
            // Check if a null or empty string has been passed in, since this
            // will cause further interrogation of the useragent to fail.
            if (String.IsNullOrWhiteSpace(userAgent))
                return false;
            
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking
            // stack.
            if (userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. 
            // This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
       private void CheckSameSite(HttpContext httpContext,CookieOptions options )
        {
            if (options.SameSite == SameSiteMode.None)

                {
                    var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                    // TODO: Use your User Agent library of choice here.
                    if (DisallowsSameSiteNone(userAgent))
                    {
                        // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
                        options.SameSite = SameSiteMode.Unspecified;
                    }
                }      

        }
    }
}
