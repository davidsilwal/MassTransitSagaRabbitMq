using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System.Reflection;
using Customer.API;
using Customer.API.Data;
using MassTransit;

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

app.MapPost("/customers/saga", async (
    CustomerForCreationDto customerForCreationDto,
    IBus bus) =>
{
    var command = new CreateCustomer(customerForCreationDto);
    await bus.Publish(command);
  //  var customer = response.Message;
  //  return Results.Created($"/customers/{customer.Id}", customer);
  return Results.Ok();
});

app.Run();