using FluentValidation;
using FluentValidation.AspNetCore;
using Meet.API;
using Meet.API.Data;
using Meet.API.Entities;
using Meet.API.Identity;
using Meet.API.Models;
using Meet.API.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);
	
	JwtOptions jwtOptions = new();
	builder.Configuration.GetSection("jwt").Bind(jwtOptions);

	// Add services to the container.
	builder.Services.AddControllers();
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

	// adding Jwt Provider
	builder.Services.AddScoped<IJwtProvider, JwtProvider>();
	// Fluent Validation
	builder.Services.AddFluentValidationAutoValidation();
	builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
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

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			// custom path and name
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetupAPI v1");
		});
	}

	app.UseAuthentication();
	app.UseHttpsRedirection();
	app.UseAuthorization();
	app.MapControllers();

	app.Run();
}
catch (Exception exception)
{
	// NLog: catch setup errors
	logger.Error(exception, "Stopped program because of exception");
	throw;
}
finally
{
	// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
	NLog.LogManager.Shutdown();
}