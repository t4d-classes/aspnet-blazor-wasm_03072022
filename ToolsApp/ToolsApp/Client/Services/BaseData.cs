

public abstract class BaseData {

  protected string baseUrl = "";

  protected string collectionUrl() => baseUrl;

  protected string elementUrl(int colorId) =>
    $"{baseUrl}/{Uri.EscapeDataString(colorId.ToString())}";
}