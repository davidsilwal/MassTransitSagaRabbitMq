using MassTransit;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.API
{
    public class CustomerCreatedMessagingConsumer : IConsumer<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedMessagingConsumer> _logger;

        public CustomerCreatedMessagingConsumer(ILogger<CustomerCreatedMessagingConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var customer = context.Message.Customer;

            _logger.LogInformation("Messaging.API Received Text: {FirstName}", customer.FirstName);

            return Task.CompletedTask;
        }
    }
}