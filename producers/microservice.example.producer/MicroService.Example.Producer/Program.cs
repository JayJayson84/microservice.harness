using System.Reflection;
using MicroService.Example.Resolver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var entryAssembly = Assembly.GetEntryAssembly();

builder.Services.AddMessageBroker(entryAssembly);
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
