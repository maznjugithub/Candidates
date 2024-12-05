using Candidates.Endpoints;
using Candidates.Models;
using Candidates.Service;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(ServiceProvider =>
{
	var Configuration = ServiceProvider.GetRequiredService<IConfiguration>();


	var Connectionstring = Configuration.GetConnectionString("DefaultConnection") ??
	throw new ApplicationException("The Connection string is null");

	return new SqlconnectionFactory(Connectionstring);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.MapCandidatesEndpoints();
app.MapCandidatesEndpoints();

app.Run();

//using Candidates.Endpoints;
//using Candidates.Service;
//using Microsoft.Data.SqlClient;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Register SqlConnectionFactory
//builder.Services.AddSingleton(serviceProvider =>
//{
//	var configuration = serviceProvider.GetRequiredService<IConfiguration>();
//	var connectionString = configuration.GetConnectionString("DefaultConnection") ??
//						   throw new ApplicationException("The connection string is null");
//	return new SqlconnectionFactory(connectionString); // Ensure correct casing and type
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//// Ensure the extension method is correctly used
//app.MapCandidatesEndpoints(); // Call the method to map candidate endpoints

//app.Run();
