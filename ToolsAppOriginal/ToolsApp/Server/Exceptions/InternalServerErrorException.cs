namespace ToolsApp.Server.Exceptions;

class InternalServerErrorException: HttpResponseException {

  public InternalServerErrorException():
    base(StatusCodes.Status500InternalServerError, "Internal Server Error") { }
}
