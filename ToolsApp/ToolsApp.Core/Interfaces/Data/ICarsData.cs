using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Core.Interfaces.Data;

public interface ICarsData
{
  Task<IEnumerable<ICar>> All();
  Task<ICar?> One(int carId);
  Task<ICar> Append(INewCar car);
  Task Replace(ICar car);
  Task Remove(int carId);
}
