using AutoMapper;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using CarModel = ToolsApp.Shared.Models.Car;
using CarDataModel = ToolsApp.Data.Models.Car;

namespace ToolsApp.Data;

public class CarsInMemoryData: ICarsData
{
  private IMapper _mapper;

  private List<CarDataModel> _cars = new List<CarDataModel>()
    {
      new() { Id=1, Make="Ford", Model="Fusion Hybrid", Year=2020, Color="blue", Price=45000 },
      new() { Id=2, Make="Tesla", Model="S", Year=2021, Color="red", Price=120000 },
    };

  public CarsInMemoryData()
  {
    var mapperConfig = new MapperConfiguration(config =>
    {
      config.CreateMap<CarDataModel, CarModel>().ReverseMap();
    });

    _mapper = mapperConfig.CreateMapper();
  }

  public Task<IEnumerable<ICar>> All()
  {
    return Task.FromResult(_cars
      .Select(c => _mapper.Map<CarDataModel, CarModel>(c))
      .AsEnumerable<ICar>());
  }

  public Task<ICar?> One(int carId)
  {
    return Task.FromResult(_cars
      .Where(c => c.Id == carId)
      .Select(c => _mapper.Map<CarDataModel, CarModel>(c))
      .Cast<ICar>()
      .SingleOrDefault());
  }


}
