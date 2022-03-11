using Microsoft.JSInterop;

namespace ToolsApp.Client.Services;

public class ScreenBlocker: IScreenBlocker
{

  private readonly IJSRuntime _js;

  public ScreenBlocker(IJSRuntime js)
  {
    _js = js;
  }

  public async Task BlockScreen()
  {
      await _js.InvokeVoidAsync("$.blockUI");
  }

  public async Task UnblockScreen()
  {
    await _js.InvokeVoidAsync("$.unblockUI");
  }
}
