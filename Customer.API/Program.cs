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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<AppDbContext>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());


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
    AppDbContext db,
    IMapper mapper,
    IBus bus,
    ILogger<Program> logger) =>
{
    var customer = mapper.Map<Customer.API.Data.Customer>(customerForCreationDto);
    db.Customers.Add(customer);
    await db.SaveChangesAsync();

    var customerDto = mapper.Map<CustomerDto>(customer);

    logger.LogInformation("publishing: {FirstName}", customerDto.FirstName);

    await bus.Publish(new CustomerCreated(customerDto));

    return Results.Created($"/customers/{customer.Id}", customer);
});


app.Run();
