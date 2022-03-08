using Microsoft.AspNetCore.Mvc;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

using ToolsApp.Shared.Models;
using ToolsApp.Server.Exceptions;


namespace ToolsApp.Server.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CarsController : ControllerBase
{
  private ILogger _logger;
  private ICarsData _data;

  public CarsController(ILogger<CarsController> logger, ICarsData data)
  {
    _logger = logger;
    _data = data;
  }

  /// <summary>
  /// Returns a list of cars.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     GET /cars
  ///     
  /// </remarks>
  /// <response code="200">List of cars</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>List of Cars</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<ICar>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<ICar>>> All() {
    try
    {
      return Ok(await _data.All());
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "All cars failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Return a car for the given id
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     GET /cars/1
  ///     
  /// </remarks>
  /// <param name="carId">Id of the car to retrieve</param>
  /// <response code="200">A valid car</response>
  /// <response code="404">No car found for the specified id</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Car</returns>
  [HttpGet("{carId:int}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ICar), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ICar>> One(int carId)
  {
    try
    {
      var car = await _data.One(carId);

      if (car is null) {
        var errorMessage = $"Unable to find car with id {carId}.";
        _logger.LogError(errorMessage);
        return NotFound(errorMessage);
      } else {
        return Ok(car);
      }
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "One car failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Append a car.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     POST /cars
  ///
  ///     Request body is a JSON serialized new car object.
  ///     
  /// </remarks>
  /// <param name="newCar">Car to append.</param>
  /// <response code="200">Appended car including the car id.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Car</returns>
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
      _logger.LogError(exc, "Append car failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Replace a car.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     PUT /cars/1
  ///
  ///     Request body is a JSON serialized car object.
  ///     
  /// </remarks>
  /// <param name="carId">Id of car to replace.</param>
  /// <param name="car">Car to append.</param>
  /// <response code="204">No Content.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Nothing</returns>
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
      var errorMessage = "Unable to find car to replace";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "Replace car failed.");
      throw new InternalServerErrorException();
    }
  }

  /// <summary>
  /// Remove a car by id.
  /// </summary>
  /// <remarks>
  /// How to call:
  /// 
  ///     DELETE /cars/1
  ///     
  /// </remarks>
  /// <param name="carId">Id of car to remove.</param>
  /// <response code="204">No Content.</response>
  /// <response code="500">Error occurred.</response>
  /// <returns>Nothing</returns>
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
      var errorMessage = "Unable to find car to remove.";
      _logger.LogError(exc, errorMessage);
      return NotFound(errorMessage);
    }
    catch (Exception exc)
    {
      _logger.LogError(exc, "Remove car failed.");
      throw new InternalServerErrorException();
    }
  }

}
