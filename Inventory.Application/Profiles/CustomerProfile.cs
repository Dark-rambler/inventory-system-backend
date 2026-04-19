using AutoMapper;
using Inventory.Application.DataTransferObjects.CustomerDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
        }
    }
}