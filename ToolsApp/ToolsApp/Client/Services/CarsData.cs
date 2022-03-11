using System.Net.Http.Json;

namespace ToolsApp.Client.Services;

public class CarsData: BaseData, ICarsData
{
  private HttpClient _http;

  public CarsData(HttpClient http) {
    _baseUrl = "v1/cars";
    _http = http;    
  }

  public async Task<IEnumerable<ICar>?> All()
  {
    return await _http.GetFromJsonAsync<Car[]>(collectionUrl());
  }

  public async Task<ICar?> One(int carId)
  {
    return await _http.GetFromJsonAsync<Car>(elementUrl(carId));
  }

  public async Task<ICar> Append(INewCar newCar)
  {
    var httpResponseMessage = await _http.PostAsJsonAsync(collectionUrl(), newCar);
    var car = await httpResponseMessage.Content.ReadFromJsonAsync<Car>();

    if (car is null) {
      throw new NullReferenceException("car came back null");
    }

    return car;
  }

  public async Task Replace(ICar car)
  {
    await _http.PutAsJsonAsync(elementUrl(car.Id), car);
  }

  public async Task Remove(int carId)
  {
    await _http.DeleteAsync(elementUrl(carId));
  }
}
