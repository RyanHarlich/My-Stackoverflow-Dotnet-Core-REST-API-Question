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
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Client.Models.HyperMediaControls;
using Microsoft.Extensions.DependencyModel;
using Library.Data;

namespace Client
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
            services.AddMvc().AddApplicationPart(typeof(Library.Controllers.InfosController).Assembly);

            services.AddControllers();

            services.AddDbContext<ClientContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ClientContext")));

            services.AddMvc()
             .AddJsonOptions(options => {
                 options.JsonSerializerOptions.IgnoreNullValues = true;
                 options.JsonSerializerOptions.WriteIndented = true;
             });

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ClientContext dbContext = serviceProvider.GetService<ClientContext>();
            services.RegisterYourLibrary(dbContext, new API.InheritedMasterNode());
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
        }
    }
}
