using Microsoft.AspNetCore.Mvc;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ToolsApp.Shared.Models;

namespace ToolsApp.Server.Controllers;
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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

  [HttpPost()]
  [Consumes("application/json")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IColor), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<IColor>> AppendColor(
    [FromBody] NewColor newColor
  ) {

    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var color = await _data.Append(newColor);
      return Created($"/colors/{color.Id}", color);
    }
    catch (Exception exc)
    {
      // Log Exception
      throw;
    }
    
  }

  [HttpPut("{colorId:int}")]
  [Consumes("application/json")]
  [ProducesResponseType(typeof(IColor), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IColor>> ReplaceColor(
    int colorId, [FromBody] Color color
  )
  {

    try
    {
      if (!ModelState.IsValid || color is null)
      {
        return BadRequest();
      }

      if (colorId != color.Id)
      {
        return BadRequest("color ids do not match");
      }

      await _data.Replace(color);

      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      // Log Exception
      return NotFound("Unable to find color to replace");
    }
    catch (Exception exc)
    {
      // Log Exception
      throw;
    }

  }

  [HttpDelete("{colorId:int}")]
  [ProducesResponseType(typeof(IColor), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IColor>> RemoveColor(
    int colorId
  )
  {

    try
    {
      await _data.Remove(colorId);

      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      // Log Exception
      return NotFound("Unable to find color to delete");
    }
    catch (Exception exc)
    {
      // Log Exception
      throw;
    }
  }
}  

