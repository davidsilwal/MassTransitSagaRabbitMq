using System.Linq.Expressions;
using MassTransit;
using Shared.Dtos;

namespace Customer.API;

public interface ICreateCustomer 
    : CorrelatedBy<Guid>
{
    CustomerDto Customer { get; }
}

public interface ICreatedCustomerEmail :
    CorrelatedBy<Guid>
{
}

public interface ICreateCustomerAudit :
    CorrelatedBy<Guid>
{
}


public class CreateCustomerConsumer : IConsumer<ICreateCustomer>
{
    public Task Consume(ConsumeContext<ICreateCustomer> context)
    {
        Console.WriteLine("CreateCustomer");

        return Task.CompletedTask;
    }
}

public class CustomerSaga : ISaga,
                            InitiatedBy<ICreateCustomer>,
                            Orchestrates<ICreatedCustomerEmail>,
                            Orchestrates<ICreateCustomerAudit>
{
    public Guid CorrelationId { get; set; }

    public CustomerDto Customer { get; set; }

    public Task Consume(ConsumeContext<ICreateCustomer> context)
    {
        Customer = context.Message.Customer;
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<ICreatedCustomerEmail> context)
    {
        Console.WriteLine("CreatedCustomerEmail");

        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<ICreateCustomerAudit> context)
    {
        Console.WriteLine("CreateCustomerAudit");

        return Task.CompletedTask;
    }


    // public Expression<Func<CustomerSaga, CreateCustomer, bool>> CorrelationExpression =>
    //     (saga, message) => saga.CorrelationId == message.Customer.Id;
}