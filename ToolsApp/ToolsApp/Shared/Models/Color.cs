namespace ToolsApp.Shared.Models;


public class NewColor: INewColor
{
  public string? Name { get; set; }
  public string? Hexcode { get; set; }
}


public class Color: NewColor, IColor
{
  public int Id { get; set; }
}
