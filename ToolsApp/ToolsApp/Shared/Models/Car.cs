using ToolsApp.Core.Interfaces.Models;

namespace ToolsApp.Shared.Models;

// returning data to the client
public class Car: ICar
{
  public int Id { get; set; }
  public string? Make { get; set; }
  public string? Model { get; set; }
  public int? Year { get; set; }
  public string? Color { get; set; }
  public decimal? Price { get; set; }
}
