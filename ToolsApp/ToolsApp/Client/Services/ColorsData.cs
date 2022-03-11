using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace ToolsApp.Client.Services;

public class ColorsData: BaseData, IColorsData
{
  private HttpClient _http;

  public ColorsData(HttpClient http) {
    _baseUrl = "v1/colors";
    _http = http;    
  }

  [JSInvokable]
  public async Task<IEnumerable<IColor>?> All()
  {
    return await _http.GetFromJsonAsync<Color[]>(collectionUrl());
  }

  public async Task<IColor?> One(int colorId)
  {
    return await _http.GetFromJsonAsync<Color>(elementUrl(colorId));
  }

  public async Task<IColor> Append(INewColor newColor)
  {
    var httpResponseMessage = await _http.PostAsJsonAsync(collectionUrl(), newColor);
    var color = await httpResponseMessage.Content.ReadFromJsonAsync<Color>();

    if (color is null) {
      throw new NullReferenceException("color came back null");
    }

    return color;
  }

  public async Task Replace(IColor color)
  {
    await _http.PutAsJsonAsync(elementUrl(color.Id), color);
  }

  public async Task Remove(int colorId)
  {
    await _http.DeleteAsync(elementUrl(colorId));
  }
}
