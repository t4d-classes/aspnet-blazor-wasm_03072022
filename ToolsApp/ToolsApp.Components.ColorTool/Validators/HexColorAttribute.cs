using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToolsApp.Components.ColorTool.Validators;
public class HexColorAttribute: ValidationAttribute
{
  private Regex _hexColorRegex;
  
  public HexColorAttribute(int hexColorLength)
  {
    _hexColorRegex = new Regex("^[0-9a-fA-F]{" + hexColorLength.ToString() + "}$");
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    var inputValue = Convert.ToString(value);

    if (inputValue is null) {
      return new ValidationResult($"{validationContext.DisplayName} is not a valid hex color");
    }

    if (!_hexColorRegex.IsMatch(inputValue))
    {
      return new ValidationResult($"{validationContext.DisplayName} is not a valid hex color");
    }

    return ValidationResult.Success;
  }
}
