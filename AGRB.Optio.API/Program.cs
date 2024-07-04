using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Domain.Interfaces;
using AGRB.Optio.Infrastructure.Repositories;
using AGRB.Optio.Application.Services;
using AGRB.Optio.Infrastructure.PerformanceImprovmentServices;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Application.Services.TransactionRelated;
using AGRB.Optio.Application.Services.StatisticServices;
using AGRB.Optio.Application.Mapper;
using AGRB.Optio.Application.Interfaces.StatisticInterfaces;
using AGRB.Optio.Domain.Services.Outer_Services;
using AGRB.Optio.Persistance.LoggerFiles;
using AGRB.Optio.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "OptioManagementSolution", Version = "v1" });
    opt.AddSecurityDefinition("auth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter  YOu token there, 'Bearer {token}'"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Auth"
            }
        },
        new string[] { }
    }
});
});

builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<IUniteOfWork, UniteOfWork>();
builder.Services.AddScoped<IFeadbackService, FeadbackService>();
builder.Services.AddScoped<IFeadbackRepository,FeadbackRepository>();
#region addScoppedManually
builder.Services.AddScoped<ICategoryRepo, CategoryOfTransactionRepos>();
builder.Services.AddScoped<IChannelRepo, ChannelRepos>();
builder.Services.AddScoped<ILocationRepo, LocationRepos>();
builder.Services.AddScoped<IMerchantRepo, MerchantRepos>();
builder.Services.AddScoped<ITransactionRepo, TransactionRepos>();
builder.Services.AddScoped<ITypeOfTransactionRepo, TypeOfTransactionRepos>();


builder.Services.AddScoped<IAdminPanelService, AdminPanelService>();
builder.Services.AddScoped<IStatisticMerchantRelatedService, StatisticMerchantRelatedService>();
builder.Services.AddScoped<IStatisticTransactionRelatedService, StatisticTransactionRelatedService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICurrencyRelatedService,CurrencyRelatedService>();
builder.Services.AddScoped<IMerchantRelatedService, MerchantRelatedService>();
builder.Services.AddScoped<ITransactionRelatedService, TransactionRelatedService>();
builder.Services.AddScoped<ILocationToMerchantRepository,LocationToMerchantRepos>();

#endregion

//var domainAssemblyServices = Assembly.Load("RGBA.Optio.Domain");
//builder.Services.AddInjectServices(domainAssemblyServices);

//var domainAssemblyRepos = Assembly.Load("RGBA.Optio.Core");
//builder.Services.AddInjectRepositories(domainAssemblyRepos);


builder.Services.AddSingleton<CacheService>();

builder.Services.AddSingleton<SmtpService>();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddDbContext<OptioDB>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OptiosString"));
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<OptioDB>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:42130",
            ValidAudience = "http://localhost:42130",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KkQl/Fp7eupD0YdLsK+ynGpEZ6g/Y0N6/J4I2V57E8E")),
        };
    });

builder.Logging.AddConsole();
builder.Logging.AddProvider(new LoggerProvider());
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddCors(options =>
{
    options.AddPolicy("RequestPipeline",
        builder =>
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.WithOrigins("https://localhost:44359")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();


if (app.Environment.IsDevelopment() ||app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("RequestPipeline");
app.MapControllers();

app.Run();
