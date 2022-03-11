using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;
using System.Threading.Tasks;
using ToolsApp.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace ToolsApp.Tests.Server;

public class ColorsControllerUnitTest
{
  [Fact]
  public async void All()
  {
    var mockLogger = new Mock<ILogger<ColorsController>>();
    var mockColorData = new Mock<IColorsData>();

    IEnumerable<IColor> mockColors = new List<Color>()
    {
      new Color() { Id = 1, Name="red", Hexcode="ff0000" },
      new Color() { Id = 2, Name="green", Hexcode="00ff00" },
      new Color() { Id = 3, Name="blue", Hexcode="0000ff" },
    };

    mockColorData
      .Setup(mock => mock.All())
      .Returns(Task.FromResult(mockColors));

    var colorsController = new ColorsController(mockLogger.Object, mockColorData.Object);

    var result = (await colorsController.All()).Result as OkObjectResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status200OK);

    var colors = result.Value as IEnumerable<IColor>;
    Assert.NotNull(colors);
    if (colors is null) return;

    Assert.Equal(3, colors.Count());

    mockColorData.Verify(mock => mock.All(), Times.Once);
  }

  [Fact]
  public async void One()
  {
    var mockLogger = new Mock<ILogger<ColorsController>>();
    var mockColorData = new Mock<IColorsData>();

    IColor mockColor = new Color() { Id = 1, Name="red", Hexcode="ff0000" };

    mockColorData
      .Setup(colorsData => colorsData.One(1))
      .Returns(Task.FromResult<IColor?>(mockColor));

    var colorsController = new ColorsController(mockLogger.Object, mockColorData.Object);

    var result = (await colorsController.One(1)).Result as OkObjectResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status200OK);

    var color = result.Value as IColor;
    Assert.NotNull(color);
    if (color is null) return;

    Assert.Equal(color, mockColor);

    mockColorData.Verify(mock => mock.One(1), Times.Once);
  }


  [Fact]
  public async void Append()
  {
    var mockLogger = new Mock<ILogger<ColorsController>>();
    var mockColorData = new Mock<IColorsData>();

    NewColor mockNewColor = new NewColor { Name="red", Hexcode="ff0000" };
    IColor mockColor = new Color { Id = 1, Name="red", Hexcode="ff0000" };

    mockColorData
      .Setup(colorsData => colorsData.Append(mockNewColor))
      .Returns(Task.FromResult(mockColor));

    var colorsController = new ColorsController(mockLogger.Object, mockColorData.Object);

    var result = (await colorsController.AppendColor(mockNewColor)).Result as CreatedResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status201Created);

    var color = result.Value as IColor;
    Assert.NotNull(color);
    if (color is null) return;

    Assert.Equal(color, mockColor);

    mockColorData.Verify(mock => mock.Append(mockNewColor), Times.Once);
  }

  [Fact]
  public async void Replace()
  {
    var mockLogger = new Mock<ILogger<ColorsController>>();
    var mockColorData = new Mock<IColorsData>();

    Color mockColor = new Color { Id=1, Name="blue", Hexcode="0000ff" };

    mockColorData
      .Setup(colorsData => colorsData.Replace(mockColor));

    var colorsController = new ColorsController(mockLogger.Object, mockColorData.Object);

    var result = (await colorsController
      .ReplaceColor(mockColor.Id, mockColor)).Result as NoContentResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status204NoContent);

    mockColorData.Verify(mock => mock.Replace(mockColor), Times.Once);
  }

  [Fact]
  public async void ReplaceColorIdMismatch()
  {
    var mockLogger = new Mock<ILogger<ColorsController>>();
    var mockColorData = new Mock<IColorsData>();

    int mockColorId = 2;
    Color mockColor = new Color { Id=1, Name="blue", Hexcode="0000ff" };

    mockColorData
      .Setup(colorsData => colorsData.Replace(mockColor));

    var colorsController = new ColorsController(mockLogger.Object, mockColorData.Object);

    var result = (await colorsController
      .ReplaceColor(mockColorId, mockColor)).Result as BadRequestObjectResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status400BadRequest);

    mockColorData.Verify(mock => mock.Replace(mockColor), Times.Never);
  }

  [Fact]
  public async void Remove()
  {
    var mockLogger = new Mock<ILogger<ColorsController>>();
    var mockColorData = new Mock<IColorsData>();

    int mockColorId = 1;

    mockColorData
      .Setup(colorsData => colorsData.Remove(mockColorId));

    var colorsController = new ColorsController(mockLogger.Object, mockColorData.Object);

    var result = (await colorsController
      .RemoveColor(mockColorId)).Result as NoContentResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status204NoContent);

    mockColorData.Verify(mock => mock.Remove(mockColorId), Times.Once);
  }

}