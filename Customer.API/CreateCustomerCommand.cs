using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.API
{
    public record CreateCustomerCommand(CustomerForCreationDto Customer);
}
