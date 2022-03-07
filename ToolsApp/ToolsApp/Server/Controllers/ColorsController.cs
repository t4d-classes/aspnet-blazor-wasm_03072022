using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ToolsApp.Shared.Models;

namespace ToolsApp.Server.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ColorsController : ControllerBase
  {

    private List<Color> _colors = new List<Color>()
    {
      new() { Id = 1, Name="red", Hexcode="ff0000" },
      new() { Id = 2, Name="green", Hexcode="00ff00" },
      new() { Id = 3, Name="blue", Hexcode="0000ff" },
    };

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Color>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Color>> All() {
      return Ok(_colors);
    }

    [HttpGet("{colorId:int}")]
    [ProducesResponseType(typeof(Color), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Color> One(int colorId)
    {
      var color = _colors.Find(c => c.Id == colorId);

      if (color is null) {
        return NotFound();
      } else {
        return Ok(color);
      }

    }
  }
}
