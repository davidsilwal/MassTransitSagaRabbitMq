using AutoMapper;
using Customer.API.Data;
using MassTransit;
using Shared.Dtos;

namespace Customer.API.Features;

//public record CreateCustomerCommand(CustomerForCreationDto Customer);

public class CreateCustomerCommandConsumer : IConsumer<CreateCustomer>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCustomerCommandConsumer(
        AppDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<CreateCustomer> context)
    {
        var customer = _mapper.Map<Customer.API.Data.Customer>(context.Message.InputCustomer);
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        var customerDto = _mapper.Map<CustomerDto>(customer);

        Console.WriteLine("Created Customer");
        await context.RespondAsync(customerDto);
    }
}