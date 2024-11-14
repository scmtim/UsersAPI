using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using UsersAPI;
using UsersAPI.Data;
using UsersAPI.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration).Destructure.With<SensitiveDataDestructuringPolicy>()
);


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();   
// Add services to the container.
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ApiUsers"));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<User>, UserValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();
