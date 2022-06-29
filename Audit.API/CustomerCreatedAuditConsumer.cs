using MassTransit;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.API
{
    public class CustomerCreatedAuditConsumer : IConsumer<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedAuditConsumer> _logger;

        public CustomerCreatedAuditConsumer(ILogger<CustomerCreatedAuditConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var customer = context.Message.Customer;

            _logger.LogInformation("Audit.API Received Text: {FirstName}", customer.FirstName);

            //  throw new Exception("uffff");
            return Task.CompletedTask;
        }
    }
}