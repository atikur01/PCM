using atikapps;
using AtikApps.ActionFilters;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Nest;
using PCM.Data;
using PCM.Hubs;
using PCM.Models;
using PCM.Repositories;
using PCM.Services;
using Serilog;
using System.Configuration;
using static System.Reflection.Metadata.BlobBuilder;

namespace PCM {
  public class Program {
    public static void Main(string[] args) {
      try {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        var settings = new ConnectionSettings(new Uri("https://24.144.112.56:9200"))
          .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
          .BasicAuthentication("elastic", "2yUqvTzesI9M=vqcegwx")
          .EnableApiVersioningHeader();

        var client = new ElasticClient(settings);
        builder.Services.AddSingleton < IElasticClient > (client);
        builder.Services.AddScoped < ElasticsearchService > (sp => {
          var elasticClient = sp.GetRequiredService < IElasticClient > ();
          return new ElasticsearchService(elasticClient, "default-Index");
        });

        Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(builder.Configuration)
          .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
          .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
          .Enrich.FromLogContext()
          .WriteTo.Seq("http://seq.atikapps.com:5341/")
          .WriteTo.Console()
          .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddControllers(options => {
          options.Filters.Add < SerilogActionFilter > ();
        });
        builder.Services.AddControllersWithViews();
        builder.Services.AddSignalR();

        builder.Services.AddCors(options => {
          options.AddPolicy("AllowAll",
            policy => {
              policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        //builder.Services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configure EF Core with SQL Server docker container
        builder.Services.AddDbContext < AppDbContext > (options =>
          options.UseSqlServer("Server=sql.bsite.net\\MSSQL2016;Database=coc13259_;User Id=coc13259_;Password=A@a11223344!;Trusted_Connection=False;TrustServerCertificate=True;"));

        builder.Services.AddScoped < UserService > ();
        builder.Services.AddSingleton < CloudinaryUploader > ();
        builder.Services.AddScoped < ICollectionRepository, CollectionRepository > ();
        builder.Services.AddScoped < CollectionService > ();

        builder.Services.AddHttpClient < SalesforceClient > ();

        // Configure the JiraConfig section
        builder.Services.Configure < JiraConfig > (builder.Configuration.GetSection("Jira"));
        builder.Services.AddSingleton < IJiraService, JiraService > ();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options => {
          options.IdleTimeout = TimeSpan.FromMinutes(30);
          options.Cookie.HttpOnly = true;
          options.Cookie.IsEssential = true;
        });

        var app = builder.Build();

        Log.Information("Application starting up...");

        app.Use(async (context, next) => {
          if (context.Request.Method == HttpMethods.Head) {
            context.Request.Method = HttpMethods.Get;

            // Make sure the response body is stripped for HEAD
            context.Response.OnStarting(() => {
              context.Response.ContentLength = 0;
              return Task.CompletedTask;
            });
          }

          await next();
        });

        if (!app.Environment.IsDevelopment()) {
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

        app.MapHub < CommentHub > ("/commentHub");

        app.Run();
      } catch (Exception ex) {
        Log.Fatal(ex, "Application start-up failed");
      } finally {
        Log.CloseAndFlush();
      }
    }
  }
}
