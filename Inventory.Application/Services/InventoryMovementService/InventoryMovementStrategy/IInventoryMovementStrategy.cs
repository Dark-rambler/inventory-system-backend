using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public interface IInventoryMovementStrategy
    {
        MovementType Type { get; }
        Task<InventoryMovement> ExecuteAsync(InventoryMovementRequest request, IInventoryMovementRepository repository, IMapper mapper);
    }
}
