using AutoMapper;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ColorModel = ToolsApp.Shared.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;

namespace ToolsApp.Data;

public class ColorsInMemoryData: IColorsData
{
  private IMapper _mapper;

  private List<ColorDataModel> _colors = new List<ColorDataModel>()
    {
      new() { Id = 1, Name="red", Hexcode="ff0000" },
      new() { Id = 2, Name="green", Hexcode="00ff00" },
      new() { Id = 3, Name="blue", Hexcode="0000ff" },
    };

  public ColorsInMemoryData()
  {
    var mapperConfig = new MapperConfiguration(config =>
    {
      config.CreateMap<INewColor, ColorDataModel>();
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

  public Task<IColor> Append(INewColor color)
  {
    var newColorDataModel = _mapper.Map<ColorDataModel>(color);
    newColorDataModel.Id = _colors.Any() ? _colors.Max(c => c.Id) + 1 : 1;

    _colors.Add(newColorDataModel);

    return Task.FromResult(
      _mapper.Map<ColorDataModel, ColorModel>(newColorDataModel) as IColor
    );
  }

  public Task<IColor?> One(int colorId)
  {
    return Task.FromResult(_colors
      .Where(c => c.Id == colorId)
      .Select(c => _mapper.Map<ColorDataModel, ColorModel>(c))
      .Cast<IColor>()
      .SingleOrDefault());
  }

  public Task Remove(int colorId)
  {
    var colorIndex = _colors.FindIndex(c => c.Id == colorId);

    if (colorIndex == -1)
    {
      throw new IndexOutOfRangeException("Color not found");
    }

    _colors.RemoveAt(colorIndex);

    return Task.CompletedTask;
  }

  public Task Replace(IColor color)
  {
    var colorDataModel = _mapper.Map<ColorDataModel>(color);

    var colorIndex = _colors.FindIndex(c => c.Id == colorDataModel.Id);

    if (colorIndex == -1) {
      throw new IndexOutOfRangeException("Color not found");
    }

    _colors[colorIndex] = colorDataModel;

    return Task.CompletedTask;
  }
}
