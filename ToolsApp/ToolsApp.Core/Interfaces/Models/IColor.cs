namespace ToolsApp.Core.Interfaces.Models;

public interface INewColor
{
  string? Name { get; set; }
  string? Hexcode { get; set; }
}

public interface IColor: INewColor
{
  int Id { get; set; }
}
