namespace ToolsApp.Core.Interfaces.Models;

public interface INewCar
{
  string? Make { get; set; }
  string? Model { get; set; }
  int? Year { get; set; }
  string? Color { get; set; }
  decimal? Price { get; set; }
}

public interface ICar: INewCar
{
  int Id { get; set; }
}
