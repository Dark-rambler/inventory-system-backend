using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.MeasureDto;

namespace Inventory.Application.Services.MeasureService
{
    public class MeasureService(IMeasureRepository repository, IMapper mapper) : IMeasureService
    {
        public async Task<List<MeasureResponse>> GetMeasuresAsync()
        {
            return mapper.Map<List<MeasureResponse>>(await repository.GetMeasuresAsync());
        }
    }
}