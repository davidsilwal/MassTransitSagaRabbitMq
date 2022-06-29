using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MediatR;
using Shared.Contracts;

namespace Customer.API
{
    public record CreateCustomerCommand(CustomerForCreationDto Customer) : IRequest<CustomerDto>;


    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(AppDbContext db,
                                            IMapper mapper,
                                            IBus bus,
                                            ILogger<CreateCustomerCommandHandler> logger)
        {
            _db = db;
            _mapper = mapper;
            _bus = bus;
            _logger = logger;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer.API.Data.Customer>(request.Customer);
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync(cancellationToken);

            var customerDto = _mapper.Map<CustomerDto>(customer);

            _logger.LogInformation("publishing: {FirstName}", customerDto.FirstName);

            await _bus.Publish(new CustomerCreated(customerDto), cancellationToken);

            return customerDto;
        }
    }
}