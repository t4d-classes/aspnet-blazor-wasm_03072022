using System.Collections.Generic;
using System.Linq;

using Moq;
using Moq.Protected;
using Xunit;
using FluentAssertions;

using ToolsApp.Core.Interfaces.Client;
using ToolsApp.Core.Interfaces.Models;
using ToolsApp.Client.Services;
using ToolsApp.Shared.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System;

namespace ToolsApp.Tests.Client;

public class ColorsDataTest
{
  [Fact]
  public async void AllColors()
  {
    IEnumerable<IColor> mockColors = new List<IColor>()
    {
      new Color() { Id = 1, Name="red", Hexcode="ff0000" },
    };

    var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

    handlerMock
      .Protected()
      .Setup<Task<HttpResponseMessage>>(
          "SendAsync",
          ItExpr.IsAny<HttpRequestMessage>(),
          ItExpr.IsAny<CancellationToken>()
      )
      .ReturnsAsync(new HttpResponseMessage()
      {
          StatusCode = HttpStatusCode.OK,
          Content = new StringContent("[{\"id\":1,\"name\":\"red\",\"hexcode\":\"ff0000\"}]"),
      })
      .Verifiable();

    var fakeHttpClient = new HttpClient(handlerMock.Object) {
      BaseAddress = new Uri("http://localhost:1234/"),
    };

    IColorsData colorsData = new ColorsData(fakeHttpClient);

    var colors = await colorsData.All();

    colors.Should().BeEquivalentTo(mockColors);

    var expectedUri = new Uri("http://localhost:1234/v1/colors");

    handlerMock.Protected().Verify(
      "SendAsync",
      Times.Exactly(1), // we expected a single external request
      ItExpr.Is<HttpRequestMessage>(req =>
          req.Method == HttpMethod.Get  // we expected a GET request
          && req.RequestUri == expectedUri // to this uri
      ),
      ItExpr.IsAny<CancellationToken>()
    );    
  }
}

