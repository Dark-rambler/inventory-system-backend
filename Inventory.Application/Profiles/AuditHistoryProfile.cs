using AutoMapper;
using Inventory.Application.DataTransferObjects.AuditHistoryDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class AuditHistoryProfile : Profile
    {
        public AuditHistoryProfile()
        {
            CreateMap<AuditHistory, AuditHistoryResponse>();
        }
    }
}