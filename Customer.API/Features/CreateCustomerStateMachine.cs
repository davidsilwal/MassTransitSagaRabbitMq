using MassTransit;

namespace Customer.API.Features;

public class CreateCustomerStateMachine : MassTransitStateMachine<CreateCustomerState>
{
    public CreateCustomerStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CreateCustomer,
            x => x.CorrelateBy(request => request.InputCustomer, context => context.Message.InputCustomer)
                  .SelectId(context => Guid.NewGuid()));

        // Event(() => CreateAudit,
        //     x => x.CorrelateBy(request => request.InputCustomer,
        //         context => context.Message.Customer));
        //
        // Event(() => CreateMessage,
        //     x => x.CorrelateBy(request => request.InputCustomer,
        //         context => context.Message.Customer));

        Initially(When(CreateCustomer)
            //   .Then(x => x.Saga.InputCustomer = x.Message.InputCustomer)
            .TransitionTo(CreatedCustomer));

        During(CreatedCustomer,
            When(CreateAudit).TransitionTo(CreatedAuditState));

        During(CreatedAuditState,
            When(CreateMessage).TransitionTo(CreatedMessageState));
        
        SetCompletedWhenFinalized();

    }


    public State Requested { get; private set; }
    public State CreatedCustomer { get; private set; }
    public State CreatedAuditState { get; private set; }
    public State CreatedMessageState { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<CreateCustomer> CreateCustomer { get; private set; }
    public Event<CreateAudit> CreateAudit { get; private set; }
    public Event<CreateMessage> CreateMessage { get; private set; }
}