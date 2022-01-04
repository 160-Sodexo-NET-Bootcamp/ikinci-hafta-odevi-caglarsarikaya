using Api.Mapping;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Core.Abstract;
using Core.Concrete;
using Core.UnitOfWork;
using Data.Context;
using Data.IUnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _webHostEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("localMssql"),
                    b =>
                    {
                        b.MigrationsAssembly("Data");
                        b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "dbo");
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                if (_webHostEnvironment.EnvironmentName == "Development")
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddDbContext<AppDbContext>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            #region DependecyInjection
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IContainerService, ContainerService>();
            services.AddScoped<IInformationService, InformationService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            db.Database.EnsureCreated();
        }
    }
}
