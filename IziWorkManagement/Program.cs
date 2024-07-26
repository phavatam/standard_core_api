using AutoMapper;
using CacheManager.Core;
using Core.Repositories.Business.Handlers;
using Core.Repositories.Business.Interface;
using Core.Repositories.Business.IRepositories;
using Core.Repositories.Business.Repositories;
using Core.Workflow.Business.Handlers;
using Core.Workflow.Business.Interface;
using Core.Workflow.Business.MappingProfile;
using EFSecondLevelCache.Core;
using IziWork.Business.MappingProfile;
using IziWork.Common.Constans;
using IziWork.Data;
using IziWork.Data.Entities;
using IziWork.Infrastructure.ServiceRegister;
using IziWorkManagement.Middleware;
using IziWorkManagement.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Filters;
using System.Configuration;
using System.Data.Entity;

var builder = WebApplication.CreateBuilder(args);
const string _keyCORS = "IziCors";

// Add services to the container.
//1. builder.Services.AddSingleton<DataContext>();
//builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddCors(p => p.AddPolicy(_keyCORS, build =>
{
    build.AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader();
}));

// Replace the existing code block with the following code
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    //.Filter.ByIncludingOnly(Matching.FromSource("Microsoft.Extensions.Logging"))
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

/*builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Database
builder.Services.AddDbContext<UpgradeApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();
#endregion
#region App Setting
builder.Services.Configure<AppSettingModel>(builder.Configuration.GetSection("AppSettings"));
#endregion
builder.Services.Configure<AuthenticationAPIMiddlewareOptions>(builder.Configuration.GetSection("AuthenticationAPIMiddlewareOptions"));


#region Middleware
#region Define interface
ServiceRegistrar.RegisterServices(builder.Services, builder.Configuration);
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
    cfg.AddProfile<MappingProfileWF>();
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IWorkflowBusiness, WorkflowBusiness>();
#endregion

#endregion

var app = builder.Build();

#region Init data
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<UpgradeApplicationContext>();
        dbContext.Database.Migrate();
        Initialize.AddMasterData(dbContext);
    }
}
catch (Exception ex) { }
#endregion

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
// enable to check API
app.UseSwagger();
app.UseSwaggerUI();
#region Middleware authen
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
#endregion

app.UseCors(_keyCORS);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();