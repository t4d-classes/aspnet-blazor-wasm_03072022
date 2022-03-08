using Microsoft.AspNetCore.Mvc;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ToolsApp.Shared.Models;
using ToolsApp.Server.Exceptions;

namespace ToolsApp.Server.Controllers;
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ColorsController : ControllerBase
{
  private ILogger _logger;
  private IColorsData _data;

  public ColorsController(ILogger<ColorsController> logger, IColorsData data)
  {
    _logger = logger;
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
  /// <response code="500">Error occurred.</response>
  /// <returns>List of Colors</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<IColor>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<IColor>>> All() {
    try
    {
      return Ok(await _data.All());
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "All colors failed.");
      throw new InternalServerErrorException();
    }
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
  /// <response code="500">Error occurred.</response>
  /// <returns>Color</returns>
  [HttpGet("{colorId:int}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IColor), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IColor>> One(int colorId)
  {
    try
    {
      var color = await _data.One(colorId);

      if (color is null) {
        var errorMessage = $"Unable to find color with id {colorId}.";
        _logger.LogError(errorMessage);
        return NotFound(errorMessage);
      } else {
        return Ok(color);
      }
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "One color failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Append a color.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     POST /colors
  ///
  ///     Request body is a JSON serialized new color object.
  ///     
  /// </remarks>
  /// <param name="newColor">Color to append.</param>
  /// <response code="200">Appended color including the color id.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Color</returns>
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
      _logger.LogError(exc, "Append color failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Replace a color.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     PUT /colors/1
  ///
  ///     Request body is a JSON serialized color object.
  ///     
  /// </remarks>
  /// <param name="colorId">Id of color to replace.</param>
  /// <param name="color">Color to append.</param>
  /// <response code="204">No Content.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Nothing</returns>
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
      var errorMessage = "Unable to find color to replace";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "Replace color failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Remove a color by id.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     DELETE /colors/1
  ///     
  /// </remarks>
  /// <param name="colorId">Id of color to remove.</param>
  /// <response code="204">No Content.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Nothing</returns>
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
      var errorMessage = "Unable to find color to remove.";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "Remove color failed.");
      throw new InternalServerErrorException();
    }
  }
}  

