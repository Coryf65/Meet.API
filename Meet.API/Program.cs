using FluentValidation;
using FluentValidation.AspNetCore;
using Meet.API;
using Meet.API.Authorization;
using Meet.API.Data;
using Meet.API.Entities;
using Meet.API.Filters;
using Meet.API.Identity;
using Meet.API.Models;
using Meet.API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Note: this section probably could be better
	JwtOptions jwtOptions = new();
	builder.Configuration.GetSection("jwt").Bind(jwtOptions);

	// Check if we have a secret set if so we bind it
	if (builder.Configuration["JwtKey"] != string.Empty)
		jwtOptions.JwtKey = builder.Configuration["JwtKey"];

	// Add services to the container. and apply the exception filter globally
	builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));
	// Add JWTOptions
	builder.Services.AddSingleton(jwtOptions);

	builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = "Bearer";
		options.DefaultScheme = "Bearer";
		options.DefaultChallengeScheme = "Bearer";
	}).AddJwtBearer(cfg =>
	{
		cfg.RequireHttpsMetadata = false;
		cfg.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = jwtOptions.JwtIssuer,
			ValidAudience = jwtOptions.JwtIssuer,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey))
		};
	});

	// TimeTrackFilter
	builder.Services.AddScoped<TimeTrackFilter>();
	// adding Jwt Provider
	builder.Services.AddScoped<IJwtProvider, JwtProvider>();
	// custom policy example
	builder.Services.AddAuthorization(options =>
	{
		options.AddPolicy("18AndOlder", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
	});
	// register the New 18 age handler
	builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
	builder.Services.AddScoped<IAuthorizationHandler, MeetupResourceOperationHandler>();
	// Fluent Validation
	builder.Services.AddFluentValidationAutoValidation();
	builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
	builder.Services.AddScoped<IValidator<MeetupQuery>, MeetupQueryValidator>();
	// Microsoft Password Hasher
	builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
	// EntityFramework
	builder.Services.AddDbContext<MeetupContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);
	// Auto Mapper
	builder.Services.AddAutoMapper(typeof(MeetupProfile));
	// Swagger/OpenAPI (https://aka.ms/aspnetcore/swashbuckle)
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(
		c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Meetup API", Version = "v1" })
	);

	// NLog: Setup NLog for Dependency injection
	builder.Logging.ClearProviders();
	builder.Host.UseNLog();

	// https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
	builder.Services.AddCors(options => 
		options.AddPolicy("FrontEndClient", builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7260/")) // only allow from specific domains
	);

	var app = builder.Build();

	// use swagger for docs
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		// custom path and name
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetupAPI v1");
	});
	
	// Use caching where denoted by a attribute
	app.UseResponseCaching();
	// allows us to serve static files from wwwroot/
	app.UseStaticFiles();
	app.UseAuthentication();
	app.UseHttpsRedirection();
	app.UseAuthorization();
	app.MapControllers();
	app.UseCors("FrontEndClient");

	app.Run();
}
catch (Exception exception)
{
	// NLog: catch setup errors
	logger.Error(exception, "Stopped program because of exception");
}
finally
{
	// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
	NLog.LogManager.Shutdown();
}