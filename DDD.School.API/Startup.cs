using DDD.School.API.Services;
using DDD.School.Commands;
using DDD.School.Persistence;
using DDD.School.Persistence.SQL;
using DDD.School.Persistence.SQL.QueryHandlers;
using DDD.School.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using DDD.School.API.Extensions;

namespace DDD.School.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContextPool<SchoolDbContext>(builder =>
            {
                if (_env.IsDevelopment())
                    builder.EnableSensitiveDataLogging(true);

                var connStr = this.Configuration.GetConnectionString("db");
                builder.UseSqlServer(connStr);
            });
            services.AddSingleton<IConnectionStringProvider>(new ConnectionStringProvider(Configuration.GetConnectionString("db")));

            services.AddSingleton<IEventSerializer, JsonEventSerializer>();

            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IStudentsRepository, StudentsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<ISchoolUnitOfWork, SchoolUnitOfWork>();

            services.RegisterAllTypes(typeof(IValidator<>), new[] { typeof(CreateCourse).Assembly }, ServiceLifetime.Scoped);

            services.AddMediatR(new[]{
                typeof(CreateCourse).Assembly,
                typeof(CoursesArchiveQueryHandler).Assembly,
            });

            services.AddSingleton<IMessagePublisher, FakeMessagePublisher>();
            services.AddScoped<IMessageProcessor, MessageProcessor>();

            services.AddSingleton(new MessageProcessorTaskOptions(TimeSpan.FromSeconds(10), 10));
            services.AddHostedService<MessagesProcessorTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseWelcomePage();
        }
    }
}
