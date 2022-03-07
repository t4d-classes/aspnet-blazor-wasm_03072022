using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ToolsApp.Shared.Models;
using ToolsApp.Data;

namespace ToolsApp.Server.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ColorsController : ControllerBase
  {
    private IColorsData _data;

    public ColorsController(IColorsData data)
    {
      _data = data;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IColor>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IColor>>> All() {
      return Ok(await _data.All());
    }

    [HttpGet("{colorId:int}")]
    [ProducesResponseType(typeof(IColor), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IColor>> One(int colorId)
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
