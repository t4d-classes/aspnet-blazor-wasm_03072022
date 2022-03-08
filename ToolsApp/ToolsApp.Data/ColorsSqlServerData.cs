
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

  public async Task<IColor> Append(INewColor color)
  {
    using var con = _dataContext.CreateConnection();

    var colorData = await con.QueryAsync<ColorDataModel>(
      "[InsertColor]",
      color,
      commandType: System.Data.CommandType.StoredProcedure
    );

    return _mapper.Map<ColorDataModel, ColorModel>(colorData.Single()) as IColor;
  }

  public async Task<IColor?> One(int colorId)
  {
    using var con = _dataContext.CreateConnection();

    var parameters = new { ColorId = colorId }; 
    var sql = "select Id, Name, Hexcode from Color where Id = @ColorId";
    var colors = await con.QueryAsync<ColorDataModel>(sql, parameters);

    return colors
      .Select(color => _mapper.Map<ColorDataModel, ColorModel>(color))
      .Cast<IColor>().SingleOrDefault();
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
