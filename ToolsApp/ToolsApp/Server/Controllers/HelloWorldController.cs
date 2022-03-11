using Microsoft.AspNetCore.Mvc;

namespace ToolsApp.Server.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class HelloWorldController: ControllerBase {

  [HttpGet]
  public ActionResult<Message> Get() {
    return Ok(new Message { Contents = "Hello, World!" });
  }

}