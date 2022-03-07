using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ToolsApp.Shared.Models;
using ToolsApp.Data;

namespace ToolsApp.Server.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ColorsController : ControllerBase
  {
    private PrimaryColorsInMemoryData _data;

    public ColorsController()
    {
      _data = new PrimaryColorsInMemoryData();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Color>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Color>>> All() {
      return Ok(await _data.All());
    }

    [HttpGet("{colorId:int}")]
    [ProducesResponseType(typeof(Color), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Color>> One(int colorId)
    {
      var color = await _data.One(colorId);

      if (color is null) {
        return NotFound();
      } else {
        return Ok(color);
      }

    }
  }
}
