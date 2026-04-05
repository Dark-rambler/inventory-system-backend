using AutoMapper;
using Inventory.Application.DataTransferObjects.MeasureDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class MeasureProfile : Profile
    {
        public MeasureProfile()
        {
            CreateMap<Measure, MeasureResponse>();
        }
    }
}