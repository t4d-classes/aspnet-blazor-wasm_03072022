namespace ToolsApp.Client.Services;

public class ColorsData : BaseData, IColorsData
{
  private HttpClient _http;

  public ColorsData(HttpClient http)
  {
    resourceUrl = "v1/colors";
    _http = http;
  }

  public async Task<IEnumerable<IColor>?> All()
  {
    return await _http.GetFromJsonAsync<Color[]>(collectionUrl());
  }

  public async Task<IColor> Append(INewColor newColor)
  {
    var response = await _http
      .PostAsJsonAsync(collectionUrl(), newColor);

    var color = await response.Content.ReadFromJsonAsync<Color>();

    if (color is null)
    {
      throw new NullReferenceException("color was not returned");
    }

    return color;
  }

  public async Task Remove(int colorId)
  {
    await _http.DeleteAsync(elementUrl(colorId));
  }
}