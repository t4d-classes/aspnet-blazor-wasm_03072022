using Microsoft.AspNetCore.Mvc;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Server.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CarsController : ControllerBase
  {
    private ICarsData _data;

    public CarsController(ICarsData data)
    {
      _data = data;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ICar>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ICar>>> All() {
      return Ok(await _data.All());
    }

    [HttpGet("{carId:int}")]
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
  }
}
