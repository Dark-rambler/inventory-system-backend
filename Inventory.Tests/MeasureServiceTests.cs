using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.MeasureDto;
using Inventory.Application.Services.MeasureService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class MeasureServiceTests
{
    private readonly Mock<IMeasureRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MeasureService _service;

    public MeasureServiceTests()
    {
        _repositoryMock = new Mock<IMeasureRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new MeasureService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetMeasuresAsync_ReturnsAllMeasures()
    {
        var measures = new List<Measure>
        {
            new() { Id = 1, Name = "Kilogram" },
            new() { Id = 2, Name = "Liter" },
            new() { Id = 3, Name = "Unit" }
        };
        var responses = measures.Select(m => new MeasureResponse(m.Id, m.Name)).ToList();

        _repositoryMock.Setup(r => r.GetMeasuresAsync())
            .ReturnsAsync(measures);
        _mapperMock.Setup(m => m.Map<List<MeasureResponse>>(measures))
            .Returns(responses);

        var result = await _service.GetMeasuresAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("Kilogram", result[0].Name);
        Assert.Equal("Liter", result[1].Name);
        Assert.Equal("Unit", result[2].Name);
    }

    [Fact]
    public async Task GetMeasuresAsync_ReturnsEmptyList_WhenNoMeasures()
    {
        _repositoryMock.Setup(r => r.GetMeasuresAsync())
            .ReturnsAsync([]);
        _mapperMock.Setup(m => m.Map<List<MeasureResponse>>(It.IsAny<List<Measure>>()))
            .Returns([]);

        var result = await _service.GetMeasuresAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
