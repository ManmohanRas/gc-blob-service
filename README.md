# ASP.NET Core 3.0 Web API Guidance Document
[]try a pull request
1. **Create New Web API Project**
Create API Projects on Development Server:(\\mcgisdev12\_workspace\webAPIs)
	The project Name will become a new folder
 

 


2. **Install Nuget Packages**
_Nuget Packages_
```
Visual Studio Install – Use Package Manager Console
Install-Package Microsoft.AspNetCore.JsonPatch -Version 3.0.0-preview6.19307.2
Install-Package Microsoft.EntityFrameworkCore -Version 3.0.0-preview6.19304.10
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 3.0.0-preview6.19304.10
Install-Package Microsoft.EntityFrameworkCore.Design -Version 3.0.0-preview6.19304.10
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.0.0-preview6.19304.10
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 3.0.0-preview6-19319-03
Install-Package Microsoft.EntityFrameworkCore.Proxies -Version 3.0.0-preview6.19304.10
Install-Package FluentValidation -Version 8.5.0-preview2
Install-Package Swashbuckle.AspNetCore -Version 5.0.0-rc2
```
**_Optional_**
```
Install-Package Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite -Version 3.0.0-preview6.19304.10
Install-Package AutoMapper -Version 8.1.1
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection -Version 6.1.1
```
3. **Set up your database connection string.**
In your appsettings.json and appsettings.Development.json files, add the ConnectionStrings snippet to the json file.
Create an environment variable on your system that contains the connection string for the application

```
"ConnectionStrings": {
    "MyDatabase": "Data Source=mcgisdb;Initial Catalog=TestDB;Persist Security Info=True;User ID=xxxx;Password=~!@#$%^;"
  },
```
Update your MyDatabase and the database connection string parameters to point to the database you want.
You can point to a different database in development and production modes.
You can have multiple database connections 

4. **Add “Models” and “Context” folders to the solution**

5. **Add PaginatedList.cs to your root folder.**
Right click on Project – Add Existing item..
	Navigate to: [](\\mcgisdev12\_workspace\webAPIs\PaginatedList.cs)

6. **Add middleware to your Startup.cs**
Modify the ConfigureServices Section as per below
```csharp
        public void ConfigureServices(IServiceCollection services)
        {

            //Add CORS and CORS policies to control which websites can connect to your API 
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .WithMethods("GET")
                        .AllowAnyHeader()
                        .WithOrigins(Environment.GetEnvironmentVariable("CORS_ORIGINS")
                }));


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options => 
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.UseMemberCasing();
                    });

            //Add Swagger to solution to document, test and explore your API 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Version = "v1",
                    Title = "My Test API V1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Morris County GIS",
                        Email = string.Empty,
                        Url = new Uri("https://morrisgisapps.co.morris.nj.us"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    } });
            });
                    // Set the comments path for the Swagger JSON and UI.
                    // **** The next 3 lines are commented out because there is 
			// a bug....swagger should work in development mode
                    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    //c.IncludeXmlComments(xmlPath);

                });
            // Uncomment this once you've created a Context class.  
            //* Be sure to add the reference to the Context folder "using MyDatabaseAPI.Context;" 

            //    services.AddDbContext<MyDatabaseContext>((options) =>
            //        {
            //            options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__gisDB"),
            //                (sqlOptions) =>
            //                {
            //                   // future option for modelling geometry data types 
            //                   // sqlOptions.UseNetTopologySuite();
            //                });
            //        });
        }
```
Modify the Configure section as per below.
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this 
		// for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            //  app.UseHealthChecks("/ready");
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseMvc();
        }
```
Add the missing references
```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MyDatabaseAPI.Context;
```
7. **Turn on XML Documentation** 

 
8. **Save and build your project** 
Run the project to ensure that all of your references are complete

9. **Scaffold your database models using Entity Framework commands** 
Scaffold-DbContext "Data Source=mcgisdb;Initial Catalog=TestDB;Persist Security Info=True;User ID=mcprima;Password=G!S#17:2:db:mcprima" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables "Lkup_Municipality","Agencies","Lkup_AgencyType" -ContextDir Context -Context MyDatabaseContext  -Force

10. **Scaffold your database controllers using Entity Framework commands.**
Right click on Project – Add New Scaffold item..


 

**Scratch content below**
dotnet ef dbcontext scaffold "Data Source=mcgisdb;Initial Catalog=PublicLandInventoryDB;Persist Security Info=True;User ID=mcprima;Password=G!S#17:2:db:mcprima" Microsoft.EntityFrameworkCore.SqlServer -o Models -t Lkup_Municipality -t Agencies -t Tracts  -t LandInventory --context-dir Context -c PLIDbContext  --data-annotations -f

**scaffold specific tables**
dotnet ef dbcontext scaffold "Server= SRICE_WIN10SUR\SQLEXPRESS;Database=POSI;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -t Municipalities -t PLIAgencies --context-dir Context -c PLIdbContext  --data-annotations -f
Scaffold-DbContext "Data Source=mcgisdb;Initial Catalog=PublicLandInventoryDB;Persist Security Info=True;User ID=mcprima;Password=G!S#17:2:db:mcprima" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables "Lkup_Municipality","Agencies","Tracts","LandInventory" -ContextDir Context -Context PLIDbContext  -Force


_**Note:**_ The scaffold command creates models with exact same name as tables.  WebAPI code will be more readable/intuitive if you rename the models in singular form (eg.  Municipalities => Municipality).  Renaming will require that you also update the Context file that was created during the scaffolding - to map the API Model name to the database Table name.


**Visual Studio Code** 
dotnet tool install --global dotnet-ef --version 3.0.0-preview6.19304.10
dotnet new webapi -o PLIapi
code -r PLIapi

dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 3.0.0-preview6.19307.2
dotnet add package Microsoft.AspNet.WebApi.Client --version 5.2.7
dotnet add package Microsoft.AspNetCore.JsonPatch --version 3.0.0-preview6.19307.2
dotnet add package Microsoft.EntityFrameworkCore --version 3.0.0-preview6.19304.10
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.0.0-preview6.19304.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.0.0-preview6.19304.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.0.0-preview6.19304.10
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 3.0.0-preview6-19319-03
dotnet add package Microsoft.EntityFrameworkCore.Proxies --version 3.0.0-preview6.19304.10
dotnet add package FluentValidation --version 8.5.0-preview2
dotnet add package Swashbuckle.AspNetCore --version 5.0.0-rc2

The next step in “wiring” up a table to the api is to create a controller that will execute the CRUD commands on the table… Typically you create one controller for each table and the controller maps to a URL address (eg. https://myCRIapi/districts).  



dotnet aspnet-codegenerator controller -name FeatureController -m Features -dc CRIContext --relativeFolderPath Controllers 
dotnet aspnet-codegenerator --project . controller -name YOUR_CONTROLLER_NAME -m YOUR_MODEL_NAME -dc YOUR_DB_CONTEXT_CLASS -outDir Controllers/


dotnet aspnet-codegenerator --project . controller -name AgencyController -m Agency -dc PLIDbContext -outDir Controllers/


dotnet ef dbcontext scaffold "Data Source=mcgisdb;Initial Catalog=PublicLandInventoryDB;Persist Security Info=True;User ID=mcprima;Password=G!S#17:2:db:mcprima" Microsoft.EntityFrameworkCore.SqlServer -o Models -t Lkup_Municipality -t Agencies -t Tracts  -t LandInventory --context-dir Context -c PLIDbContext  --data-annotations -f

Lookup Tables

https://stackoverflow.com/questions/28439176/how-to-return-results-from-a-lookup-table-in-my-mvc5-controller-and-view
