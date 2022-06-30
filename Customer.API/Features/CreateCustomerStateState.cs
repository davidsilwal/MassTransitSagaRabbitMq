using MassTransit;
using Shared.Dtos;

namespace Customer.API.Features;

public abstract class CreateCustomerState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    
    public CustomerForCreationDto InputCustomer { get; set; }
    
    public int Version { get; set; }
}