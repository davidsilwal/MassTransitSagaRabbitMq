using AutoMapper;
using Customer.API.Data;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Dtos;
using System.Reflection;
using Customer.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddBus(builder.Configuration);

var app = builder.Build();
AppDbContext.Initialize(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/", () => "Hello World");

app.MapGet("/customers", async (AppDbContext db) =>
    await db.Customers.ToListAsync());

app.MapGet("/customers/{id}", async (int id, AppDbContext db) =>
    await db.Customers.FindAsync(id)
        is Customer.API.Data.Customer customer
        ? Results.Ok(customer)
        : Results.NotFound());


app.MapPost("/customers", async (
    CustomerForCreationDto customerForCreationDto,
    IMediator mediator) =>
{
    var command = new CreateCustomerCommand(customerForCreationDto);
    var response = await mediator.Send(command);
    return Results.Created($"/customers/{response.Id}", response);
});


app.Run();