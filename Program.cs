using Microsoft.EntityFrameworkCore;
using PCM.ActionFilter;
using PCM.Data;
using PCM.Services;
using Serilog;

namespace PCM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)  // Reads configuration from appsettings.json
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)  // Logs from Microsoft namespace
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)  // Logs from System namespace
                .Enrich.FromLogContext()  // Enrich logs with context information
                .WriteTo.Seq("http://seq.atikapps.com:5341/")
                .WriteTo.Console()  // Console sink for debugging              
                .CreateLogger();

            // Replace the default logging with Serilog
            builder.Host.UseSerilog();

            // Register the Serilog action filter globally
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<SerilogActionFilter>();
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });


            // Configure EF Core with SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Register UserService as a scoped dependency
            builder.Services.AddScoped<UserService>();

            builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddSession();



            try
            {
                var app = builder.Build();

                Log.Information("Application starting up...");

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();


                app.UseCors("AllowAll");

                app.UseAuthorization();
                app.UseSession();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Collection}/{action=Create}/{id?}");


                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();


            }

        }
    }
    
}
