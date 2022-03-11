using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Core.Interfaces.Client;

public interface IColorsData {
  Task<IEnumerable<IColor>?> All();
  Task<IColor?> One(int colorId);
  Task<IColor> Append(INewColor newColor);
  Task Replace(IColor color);
  Task Remove(int colorId);
}