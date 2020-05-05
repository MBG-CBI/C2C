using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Concrete;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;
using MOBOT.DigitalAnnotations.Business.Services;
using MOBOT.DigitalAnnotations.Data.Context;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace MOBOT.DigitalAnnotations.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
        {            
            Configuration = config;
            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogWarning($"Environment: {env.EnvironmentName}");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("AnnotationsConnection");
            services.AddEntityFrameworkSqlServer().AddEntityFrameworkProxies()
                .AddDbContext<DataContext>((serviceProvider, options) => {
                    options.UseLazyLoadingProxies();
                    options.UseSqlServer(connectionString)
                        .UseInternalServiceProvider(serviceProvider);                    
                     });
            services.AddScoped<IUnitOfWork, DataContext>();
            services.AddTransient<ITokenProvider, RerumTokenProvider>();
            services.AddScoped<IAnnotationService, AnnotationService>();
            services.AddScoped<IAnnotationSourceService, AnnotationSourceService>();
            services.AddScoped<IAnnotationTargetService, AnnotationTargetService>();
            services.AddScoped<IWebAnnotationService, WebAnnotationService>();
            services.AddScoped<IWebManifestService, WebManifestService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<IAnnotationTypeService, AnnotationTypeService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IAnnotationFilterService, AnnotationFilterService>();
            services.AddScoped<IVocabularySearchService, VocabularySearchService>();
            services.AddSingleton((IConfigurationRoot)Configuration);           
            services.AddMemoryCache();
            services.AddSingleton<ITokenManager<RerumToken>, TokenManagerService>();
            services.AddScoped<IRerumCommunicator, RerumCommunicatorService>();
            services.AddCors(opts => {
                opts.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            //var serializerSettings = new 
            services.AddMvc()
                .AddJsonOptions(opts => opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)
                .AddJsonOptions(opts => opts.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "MOBOT Digital Annotations Api",
                    Version = "v1",
                    Description = "API to service the Digital Annotations Web Interface."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {            
            var nlogFilePath = "nlog.config";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                nlogFilePath = $"nlog.{env.EnvironmentName}.config";
            }
            NLog.LogManager.LoadConfiguration(nlogFilePath);

            app.UseStaticFiles();

            app.UseSwagger(c => { c.RouteTemplate = "api/docs/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c=> {
                                
                c.RoutePrefix = "api/docs";
                c.SwaggerEndpoint("./v1/swagger.json", "MOBOT Digital Annotations Api");
            });
            app.UseCors("CorsPolicy");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
