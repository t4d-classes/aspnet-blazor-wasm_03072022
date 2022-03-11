namespace ToolsApp.Shared.Models;


public class NewCar: INewCar {
  public string? Make { get; set; }
  public string? Model { get; set; }
  public int? Year { get; set; }
  public string? Color { get; set; }
  public decimal? Price { get; set; }
}


public class Car: NewCar, ICar
{
  public int Id { get; set; }
}
