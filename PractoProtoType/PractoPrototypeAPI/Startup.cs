using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PractoPrototypeAPI.Logic;
using PractoPrototypeAPI.Logic.BusinessLogic.Doctor;
using PractoPrototypeAPI.Repository;
using PractoPrototypeAPI.Repository.Repository;
using System;

namespace PractoPrototypeAPI
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddSingleton<IConfiguration>(Configuration);
            AddTransient(services);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PractoPrototype UI",
                    Description = "PractoPrototype Swagger UI",
                });
            });
            services.AddControllers();
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {               
                options.Authority = Configuration[Constants.OktaAuthority];
                options.RequireHttpsMetadata = Convert.ToBoolean(Configuration[Constants.OktaRequireHttpsMetadata]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Showing API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
        }

        private void AddTransient(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPatentReportRepository, PatentReportRepository>();
            services.AddTransient<IPatentRepository, PatentRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IPatenetLogic, PatenetLogic>();
            services.AddTransient<IDoctorLogic, DoctorLogic>();
            services.AddTransient<ITest, Test>();
        }
    }
}
