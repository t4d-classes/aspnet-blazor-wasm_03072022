using AutoMapper;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ColorModel = ToolsApp.Shared.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;

namespace ToolsApp.Data;

public class PrimaryColorsInMemoryData: IColorsData
{
  private IMapper _mapper;

  private List<ColorDataModel> _colors = new List<ColorDataModel>()
    {
      new() { Id = 1, Name="red", Hexcode="ff0000" },
      new() { Id = 2, Name="green", Hexcode="00ff00" },
      new() { Id = 3, Name="blue", Hexcode="0000ff" },
    };

  public PrimaryColorsInMemoryData()
  {
    var mapperConfig = new MapperConfiguration(config =>
    {
      config.CreateMap<ColorDataModel, ColorModel>().ReverseMap();
    });

    _mapper = mapperConfig.CreateMapper();
  }

  public Task<IEnumerable<IColor>> All()
  {
    return Task.FromResult(_colors
      .Select(c => _mapper.Map<ColorDataModel, ColorModel>(c))
      .AsEnumerable<IColor>());
  }

  public Task<IColor?> One(int colorId)
  {
    return Task.FromResult(_colors
      .Where(c => c.Id == colorId)
      .Select(c => _mapper.Map<ColorDataModel, ColorModel>(c))
      .Cast<IColor>()
      .SingleOrDefault());
  }


}
