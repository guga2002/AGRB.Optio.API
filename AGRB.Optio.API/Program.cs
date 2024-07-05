#region Usings
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AGRB.Optio.Domain.Interfaces;
using AGRB.Optio.Infrastructure.Repositories;
using AGRB.Optio.Infrastructure.PerformanceImprovmentServices;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Application.Mapper;
using AGRB.Optio.Domain.Services.Outer_Services;
using AGRB.Optio.Persistance.LoggerFiles;
using AGRB.Optio.Domain.Data;
using AGRB.Optio.API.CustomMiddlwares;
using AGRB.Optio.API.CustomMiddlwares.AGRB.Optio.API.CustomMiddlwares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Reflection;
using AGRB.Optio.Persistance.Reflections;
#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(DateTime.Now);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
});

#region Swagger config
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AGRB.Optio.API",
        Description = "AGRB.Optio.API is a powerful RESTful API project developed for the optimization and management of Bank Transactions.\r\nThis API provides multi-functional operations and facilitates the automation of data management and analysis processes.",
        TermsOfService = new Uri("https://github.com/guga2002/AGRB.Optio.API/blob/master/README.md"),
        Contact = new OpenApiContact
        {
            Name = "Contact Me",
            Url = new Uri("https://www.linkedin.com/in/guga-apkhazava-938a40237/")
        },
        License = new OpenApiLicense
        {
            Name = "License, Source Code",
            Url = new Uri("https://github.com/guga2002/AGRB.Optio.API.git")
        }
    });

    //opt.DocInclusionPredicate((docName, apiDesc) =>
    //{
    //    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;
    //    var versions = methodInfo.DeclaringType.GetCustomAttributes(true)
    //        .OfType<ApiVersionAttribute>()
    //        .SelectMany(attr => attr.Versions);

    //    var maps = apiDesc.ActionDescriptor.AttributeRouteInfo?.Template
    //        .Split('/')
    //        .SelectMany(sub => sub.Split('.'))
    //        .Distinct();
    //    Console.WriteLine(docName);
    //    return versions.Any(v => $"v{v}" == docName+".0"); //&&
    //    //(!maps.Any() || maps.Contains(docName));
    //    //return true;
    //});


    opt.AddSecurityDefinition("auth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter token here"
    });
    opt.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "auth"
                }
            },
            Array.Empty<string>()
                }
            });
});
#endregion

#region Services Lifetime
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<IUniteOfWork, UniteOfWork>();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<SmtpService>();

#region addScoppedManually
//builder.Services.AddScoped<ICategoryRepo, CategoryOfTransactionRepos>();
//builder.Services.AddScoped<IChannelRepo, ChannelRepos>();
//builder.Services.AddScoped<ILocationRepo, LocationRepos>();
//builder.Services.AddScoped<IMerchantRepo, MerchantRepos>();
//builder.Services.AddScoped<ITransactionRepo, TransactionRepos>();
//builder.Services.AddScoped<ITypeOfTransactionRepo, TypeOfTransactionRepos>();
//builder.Services.AddScoped<ILocationToMerchantRepository,LocationToMerchantRepos>();
//builder.Services.AddScoped<IFeadbackRepository, FeadbackRepository>();
#endregion



var applicatinoAssemblyServices = Assembly.Load("AGRB.Optio.Application");
builder.Services.AddInjectServices(applicatinoAssemblyServices);


var interfaceAssembly = Assembly.Load("AGRB.Optio.Domain");
var implementationAssembly = Assembly.Load("AGRB.Optio.Infrastructure");
builder.Services.AddInjectRepositories(interfaceAssembly, implementationAssembly);
#endregion

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

#region Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

#region DbContext
builder.Services.AddDbContext<OptioDB>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OptiosString"));
});
#endregion

#region Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<OptioDB>()
    .AddDefaultTokenProviders();
#endregion

#region Authentification
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
#endregion

#region Logger Configuration
builder.Logging.AddConsole();
builder.Logging.AddProvider(new LoggerProvider());
builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endregion

#region Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = "OptioSOlutionCookie";
    options.LoginPath = "/Customer/SignIn";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});
#endregion

#region Cors
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
#endregion

var app = builder.Build();


if (app.Environment.IsDevelopment() ||app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AGRB.Optio.API v1");
        c.RoutePrefix = "swagger";
    });
};

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("RequestPipeline");
app.MapControllers();

#region Custom Middlwares
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
#endregion

app.Run();
