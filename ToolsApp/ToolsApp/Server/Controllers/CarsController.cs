using Microsoft.AspNetCore.Mvc;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ToolsApp.Shared.Models;


namespace ToolsApp.Server.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CarsController : ControllerBase
{
  private ICarsData _data;

  public CarsController(ICarsData data)
  {
    _data = data;
  }

  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<ICar>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<ICar>>> All() {
    return Ok(await _data.All());
  }

  [HttpGet("{carId:int}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> One(int carId)
  {
    var car = await _data.One(carId);

    if (car is null) {
      return NotFound();
    } else {
      return Ok(car);
    }

  }

  [HttpPost()]
  [Consumes("application/json")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<ICar>> AppendCar(
    [FromBody] NewCar newCar
  )
  {

    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var car = await _data.Append(newCar);
      return Created($"/cars/{car.Id}", car);
    }
    catch (Exception exc)
    {
      // Log Exception
      throw;
    }

  }


  [HttpPut("{carId:int}")]
  [Consumes("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> ReplaceCar(
    int carId, [FromBody] Car car
  )
  {

    try
    {
      if (!ModelState.IsValid || car is null)
      {
        return BadRequest();
      }

      if (carId != car.Id)
      {
        return BadRequest("car ids do not match");
      }

      await _data.Replace(car);

      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      // Log Exception
      return NotFound("Unable to find car to replace");
    }
    catch (Exception exc)
    {
      // Log Exception
      throw;
    }

  }

  [HttpDelete("{carId:int}")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> RemoveCar(
    int carId
  )
  {

    try
    {
      await _data.Remove(carId);

      return NoContent();
    }
    catch (IndexOutOfRangeException exc)
    {
      // Log Exception
      return NotFound("Unable to find car to delete");
    }
    catch (Exception exc)
    {
      // Log Exception
      throw;
    }

  }


}
