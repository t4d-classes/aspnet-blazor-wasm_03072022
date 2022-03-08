
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ColorModel = ToolsApp.Shared.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;

using AutoMapper;
using Dapper;

namespace ToolsApp.Data;

public class ColorsSqlServerData : IColorsData
{
  private DataContext _dataContext;
  private IMapper _mapper;

  public ColorsSqlServerData(DataContext dataContext)
  {
    _dataContext = dataContext;
    var mapperConfig = new MapperConfiguration(config =>
    {
      config.CreateMap<ColorDataModel, ColorModel>().ReverseMap();
    });
    _mapper = mapperConfig.CreateMapper();
  }

  public async Task<IEnumerable<IColor>> All()
  {
    using var con = _dataContext.CreateConnection();

    var sql = "select Id, Name, Hexcode from Color";
    var colors = await con.QueryAsync<ColorDataModel>(sql);

    return colors
      .Select(color => _mapper.Map<ColorDataModel, ColorModel>(color))
      .AsEnumerable<IColor>();

  }

  public Task<IColor> Append(INewColor color)
  {
    throw new NotImplementedException();
  }

  public Task<IColor?> One(int colorId)
  {
    throw new NotImplementedException();
  }

  public Task Remove(int colorId)
  {
    throw new NotImplementedException();
  }

  public Task Replace(IColor color)
  {
    throw new NotImplementedException();
  }
}
