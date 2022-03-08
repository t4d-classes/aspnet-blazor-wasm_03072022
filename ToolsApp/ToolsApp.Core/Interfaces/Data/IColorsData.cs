using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Core.Interfaces.Data;

public interface IColorsData
{
  Task<IEnumerable<IColor>> All();
  Task<IColor?> One(int colorId);
  Task<IColor> Append(INewColor color);
  Task Replace(IColor color);
  Task Remove(int colorId);
}
