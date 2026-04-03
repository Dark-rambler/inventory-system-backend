using AutoMapper;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class ProductWarehouseProfile: Profile
    {
        public ProductWarehouseProfile()
        {
            CreateMap<WarehouseProduct, WarehouseProductResponse>();
        }
    }
}
