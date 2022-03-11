using Microsoft.AspNetCore.Http;

namespace ToolsApp.Tests.Server;

public class HelloWorldControllerUnitTest
{
  [Fact]
  public void GetMessage()
  {

    var helloWorldController = new HelloWorldController();

    var result = helloWorldController.Get().Result as OkObjectResult;
    Assert.NotEqual(null, result);
    if (result is null) return;

    Assert.Equal(result.StatusCode, StatusCodes.Status200OK);

    var message = result.Value as Message;
    Assert.NotEqual(null, message);
    if (message is null) return;

    Assert.Equal("Hello, World!", message.Contents);
  }
}