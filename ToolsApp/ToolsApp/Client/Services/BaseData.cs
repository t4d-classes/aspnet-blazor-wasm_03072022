

public abstract class BaseData {

  protected string _baseUrl = "";

  protected string collectionUrl() => _baseUrl;

  protected string elementUrl(int colorId) =>
    $"{_baseUrl}/{Uri.EscapeDataString(colorId.ToString())}";
}