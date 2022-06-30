using MassTransit;

namespace Messaging.API;

public class CreateMessageConsumer : IConsumer<CreateMessage>
{
    public Task Consume(ConsumeContext<CreateMessage> context)
    {
        Console.WriteLine("CreateMessageConsumer");
        return Task.CompletedTask;
    }
}