using System.Diagnostics;
using Hub.Transactions.WebAPI.Data;
using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Filters;
using Hub.Transactions.WebAPI.Logs;
using Hub.Transactions.WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using PetaPoco;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Threading.Tasks;
using StackExchange.Profiling;
using static Hub.Transactions.WebAPI.Logs.LogHelper;

namespace Hub.Transactions.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            MiniProfiler.Settings.SqlFormatter =
                new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

            Configuration.CreateLogger(new Masker(
                Configuration["MaskFields"],
                Configuration["MaskValue"]));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMasker>(x => new Masker(
                Configuration["MaskFields"],
                Configuration["MaskValue"]));

            services.AddScoped<IDatabase>(provider =>
            {
                var connectionString = Configuration.GetConnectionString("ReportingPostgreSQLConnectionString");
                var database = new ProfiledDatabase(connectionString, "Npgsql");

                return database;
            });
            services.AddScoped<IStatementDBExtensions>(provider =>
            {
                var connectionString = Configuration.GetConnectionString("StatementPostgreSQLConnectionString");
                var database = new ProfiledDatabase(connectionString, "Npgsql");
                return database;
            });
            services.AddSingleton<IConfiguration>(Configuration);

            // Add framework services.
            services.AddMvc().AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Add Swagger 
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info { Title = "Hub Transactions API", Version = "v1" });
                options.OperationFilter<MethodNamesOperationFilter>();

                var authenticationTokenDefined = Configuration["AuthenticationToken"].NotNullOrEmpty();

                if (authenticationTokenDefined)
                {
                    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header"
                    });   
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandler != null && exceptionHandler.Error != null)
                    {
                        await Task.Run(() => Log.ForContext("Exception", exceptionHandler.Error, true).Error("{Message}", exceptionHandler.Error.Message));
                    }
                });
            });


            var sw = new Stopwatch();

            appLifetime.ApplicationStarted.Register(LogApplicationStarted);

            appLifetime.ApplicationStopped.Register(LogApplicationStopped);

            app.UseMiddleware<LogMiddleware>();

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseMvc();

            // Enable Swagger 
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hub Transactions API v1");
            });

            var enableMigration = Configuration["EnableReportingPostgreSQLMigration"].ToBool();

            if (enableMigration)
            {
                var connectionString = Configuration.GetConnectionString("ReportingPostgreSQLConnectionString");
                var database = new Database(connectionString, "Npgsql");

                Migrator
                    .ForDatabase(new PostgreSQLMigrationDatabase(database))
                    .WithPath(Path.Combine(env.ContentRootPath, "Migrations"))
                    .Migrate();
            }
        }
    }
}
