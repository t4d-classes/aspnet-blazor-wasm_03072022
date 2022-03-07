using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Shared.Models;

// returning data to the client
public class Color: IColor
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Hexcode { get; set; }
}
