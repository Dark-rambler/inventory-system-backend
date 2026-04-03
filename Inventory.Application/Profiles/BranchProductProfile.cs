using AutoMapper;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class BranchProductProfile : Profile
    {
        public BranchProductProfile()
        {
            CreateMap<BranchProduct, BranchProductResponse>();
        }
    }
}
