using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Core.Interfaces.Data;

public interface ICarsData
{
  Task<IEnumerable<ICar>> All();
  Task<ICar?> One(int carId);
}
