using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapper;
using Xunit;
using Moq;
using Moq.Dapper;
using FluentAssertions;

using ToolsApp.Data;
using ToolsApp.Core.Interfaces.Models;

using Color = ToolsApp.Shared.Models.Color;
using ColorDataModel = ToolsApp.Data.Models.Color;


namespace ToolsApp.Tests.Data;


public class ColorsSqlServerDataTest
{
    [Fact]
    public async void AllColorsTest()
    {
      IEnumerable<ColorDataModel> mockDataColors = new List<ColorDataModel>()
      {
        new ColorDataModel() { Id = 1, Name="red", Hexcode="ff0000" },
        new ColorDataModel() { Id = 2, Name="green", Hexcode="00ff00" },
        new ColorDataModel() { Id = 3, Name="blue", Hexcode="0000ff" },
      };

      IEnumerable<IColor> expectedColors = new List<IColor>()
      {
        new Color() { Id = 1, Name="red", Hexcode="ff0000" },
        new Color() { Id = 2, Name="green", Hexcode="00ff00" },
        new Color() { Id = 3, Name="blue", Hexcode="0000ff" },
      };

      var mockSqlConnection = new Mock<IDbConnection>();

      var sql = "select Id, Name, Hexcode from Color";

      mockSqlConnection
        .SetupDapperAsync(c =>
          c.QueryAsync<ColorDataModel>(
            sql,
            It.IsAny<object>(),
            It.IsAny<IDbTransaction>(),
            It.IsAny<int>(),
            It.IsAny<CommandType>())
        )
        .ReturnsAsync(mockDataColors);

      var mockDataContext = new Mock<DataContext>();

      mockDataContext.Setup(dataContext =>
        dataContext.CreateConnection()).Returns(mockSqlConnection.Object);

      var colorsData = new ColorsSqlServerData(mockDataContext.Object);

      IEnumerable<IColor> colorModels = await colorsData.All();

      Assert.Equal(expectedColors.Count(), colorModels.Count());

      colorModels.Should().BeEquivalentTo(expectedColors);
    }
}