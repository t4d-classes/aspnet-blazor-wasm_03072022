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

    /// <summary>
    /// Returns a list of colors
    /// </summary>
    /// <remarks>
    /// How to call:
    /// 
    ///     GET /colors
    ///     
    /// </remarks>
    /// <response code="200">List of Colors</response>
    /// <returns>List of Colors</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<IColor>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IColor>>> All() {
      return Ok(await _data.All());
    }

    /// <summary>
    /// Return a color for the given id
    /// </summary>
    /// <remarks>
    /// How to call:
    /// 
    ///     GET /colors/1
    ///     
    /// </remarks>
    /// <param name="colorId">Id of the color to retrieve</param>
    /// <response code="200">A valid color</response>
    /// <response code="404">No color found for the specified id</response>
    /// <returns>Color</returns>
    [HttpGet("{colorId:int}")]
    [Produces("application/json")]
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
