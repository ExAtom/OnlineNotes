using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineNotes.Data;
using OnlineNotes.Data.Repositories;
using OnlineNotes.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtConfig = builder.Configuration.GetSection("JwtConfig");

builder.Services.Configure<MongoDbConfig>(
	builder.Configuration.GetSection("MongoDbConfig")
);
builder.Services.Configure<JwtConfig>(jwtConfig);

builder.Services.AddSingleton<NoteRepository>();
builder.Services.AddSingleton<UserRepository>();

builder.Services.AddControllers();

#if DEBUG
builder.Services.AddCors(option =>
{
	option.AddPolicy("EnableCORS", builder =>
	{
		builder.SetIsOriginAllowed(origin => true)
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials()
			.Build();
	});
});
#endif

builder.Services
	.AddAuthentication(option =>
	{
		option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(option =>
	{
		option.SaveToken = true;
		option.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.ASCII.GetBytes(jwtConfig.Get<JwtConfig>().Secret)
			),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
#if DEBUG
app.UseCors("EnableCORS");
#endif

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
