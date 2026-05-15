using Inventory.Application.DataTransferObjects.MeasureDto;

namespace Inventory.Application.Services.MeasureService
{
    public interface IMeasureService
    {
        Task<List<MeasureResponse>> GetMeasuresAsync();
    }
}
