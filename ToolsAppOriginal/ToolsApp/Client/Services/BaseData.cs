namespace ToolsApp.Client.Services;

public abstract class BaseData
{
  protected string resourceUrl = "";

  protected string collectionUrl() => resourceUrl;

  protected string elementUrl(int elementId) =>
    $"{resourceUrl}/{Uri.EscapeDataString(elementId.ToString())}";
}