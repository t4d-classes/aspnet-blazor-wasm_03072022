using Microsoft.AspNetCore.Http;

namespace ToolsApp.Tests.Server;

public class HelloWorldControllerUnitTest
{
  [Fact]
  public void GetMessage()
  {

    var helloWorldController = new HelloWorldController();

    var result = helloWorldController.Get().Result as OkObjectResult;
    Assert.NotNull(result);
    if (result is null) return;

    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

    var message = result.Value as Message;
    Assert.NotEqual(null, message); // Not Null is preferred
    if (message is null) return;

    Assert.Equal("Hello, World!", message.Contents);
  }
}