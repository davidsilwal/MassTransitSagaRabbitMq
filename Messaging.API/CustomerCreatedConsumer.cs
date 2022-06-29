using MassTransit;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.API
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedConsumer> _logger;

        public CustomerCreatedConsumer(ILogger<CustomerCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var customer = context.Message.Customer;

            _logger.LogInformation("Received Text: {FirstName}", customer.FirstName);

            return Task.CompletedTask;

        }
    }
}
