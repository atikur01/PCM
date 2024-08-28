using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Nest;
using PCM.Data;
using PCM.Hubs;
using PCM.Models;
using PCM.Services;
using Serilog;
using static System.Reflection.Metadata.BlobBuilder;
using atikapps;
using AtikApps.ActionFilters;

namespace PCM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddHttpContextAccessor();

                // Configure Elasticsearch
                var settings = new ConnectionSettings(new Uri("https://24.144.112.56:9200"))
                   .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                   .BasicAuthentication("elastic", "2yUqvTzesI9M=vqcegwx")
                   .EnableApiVersioningHeader();

                var client = new ElasticClient(settings);
                builder.Services.AddSingleton<IElasticClient>(client);

                builder.Services.AddScoped<ElasticsearchService>(sp =>
                {
                    var elasticClient = sp.GetRequiredService<IElasticClient>();
                    return new ElasticsearchService(elasticClient, "default-Index");
                });



                // Configure Serilog
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)  // Logs from Microsoft namespace
                    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)  // Logs from System namespace
                    .Enrich.FromLogContext()  // Enrich logs with context information
                    .WriteTo.Seq("http://seq.atikapps.com:5341/")
                    .WriteTo.Console()
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
                builder.Services.AddSignalR();

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


                //builder.Services.AddDbContext<AppDbContext>(options =>
                //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                // Configure EF Core with SQL Server docker container
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=167.99.127.42,1433;Database=CollectionManagement;User Id=sa;Password=67EVzrft;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;"));


                // Register UserService as a scoped dependency
                builder.Services.AddScoped<UserService>();

                // Register CloudinaryUploader as a singleton
                builder.Services.AddSingleton<CloudinaryUploader>();


                // Add distributed memory cache and session services
                builder.Services.AddDistributedMemoryCache();
                builder.Services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });


                var app = builder.Build();

                Log.Information("Application starting up...");

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapHub<CommentHub>("/commentHub");

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
