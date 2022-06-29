using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shared.Dtos;

namespace Customer.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerForCreationDto, Customer.API.Data.Customer>();
            CreateMap<Customer.API.Data.Customer, CustomerDto>();
        }
    }
}
