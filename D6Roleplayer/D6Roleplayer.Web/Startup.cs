using D6Roleplayer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using D6Roleplayer.Infrastructure.Models;
using D6Roleplayer.Web.Services;
using D6Roleplayer.Web.Hubs;
using D6Roleplayer.Infrastructure.Clients;

namespace D6Roleplayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        string connectionString = "DevConnection";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); 
            services.AddSignalR(hubOptions =>
            {
                hubOptions.MaximumReceiveMessageSize = 512000;
            });
            
            services.AddDbContext<DatabaseContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString(connectionString)));

            services.AddTransient<IDiceRollService, DiceRollService>();
            services.AddTransient<IRoleplaySessionService, RoleplaySessionService>();
            services.AddTransient<IDiceRollRepository, DiceRollRepository>();
            services.AddTransient<IInitiativeRollRepository, InitiativeRollRepository>();
            services.AddTransient<IRoleplaySessionRepository, RoleplaySessionRepository>();
            services.AddTransient<IDiceRollerClient, DiceRollerClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                connectionString = "DevConnection";
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                connectionString = "PrdConnection";
            }

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var httpConnectionDispatcherOptions = new HttpConnectionDispatcherOptions
            {
                ApplicationMaxBufferSize = 100
            };

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<DiceRollHub>("/diceRollHub");
            });
        }
    }
}