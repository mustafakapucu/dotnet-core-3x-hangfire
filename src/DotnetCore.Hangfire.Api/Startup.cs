using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hangfire;
using Hangfire.SqlServer;

namespace DotnetCore.Hangfire.Api
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
            services.AddControllers();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            HangfireJobsRegister();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }

        private void HangfireJobsRegister()
        {
            //Fire and Forgot Job
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire and Forgot Job"));

            //Schedule Job, run after xx time
            BackgroundJob.Schedule(() => Console.WriteLine("Scheduled after 5 seconds"), TimeSpan.FromSeconds(5));

            //Recurring Daily 15:10
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Daily 15:10" + DateTime.Now), Cron.Daily(15, 10));

            //Recurring every minute
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring every minute" + DateTime.Now), Cron.Minutely());

            //Recurring every 5 minute
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring every minute" + DateTime.Now), "5 * * * *");

        }
    }
}
