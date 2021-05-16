using System.Threading.Tasks;
using EarlyLearning.API.DependencyInjection;
using EarlyLearning.API.Fakes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using CurrentUser = EarlyLearning.API.Dataclasses.User.CurrentUser;

namespace EarlyLearning.API
{
    public partial class Startup
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _logger = Log.Logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            EarlyLearningAPI.BuildDependencies(services);
            services.AddTransient<CurrentUser, CurrentFakeUser>();
            services.AddSingleton(_logger);
            Task.WaitAll(ConfigureRavenDb(services));
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
            SetupCors(app);


            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupCors(IApplicationBuilder app)
        {
            var corsOrigins = Configuration["CORS"];
            app.UseCors(options =>
            {
                options.WithOrigins(
                        corsOrigins)
                    .AllowAnyHeader().AllowAnyMethod();
            });
            Log.ForContext("Origins", corsOrigins).Information("Setting up CORS");
        }
    }
}
