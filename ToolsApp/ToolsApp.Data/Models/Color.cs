using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsApp.Data.Models
{
  // accessing the data source
  public class Color
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Hexcode { get; set; }
  }
}
