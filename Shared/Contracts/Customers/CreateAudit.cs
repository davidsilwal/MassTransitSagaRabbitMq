using MassTransit;
using Shared.Dtos;

public record CreateCustomer(CustomerForCreationDto InputCustomer) : CorrelatedBy<CustomerDto>
{
    public CustomerDto CorrelationId { get; }
}

public record CreateAudit(CustomerDto Customer);

public record CreateMessage(CustomerDto Customer);