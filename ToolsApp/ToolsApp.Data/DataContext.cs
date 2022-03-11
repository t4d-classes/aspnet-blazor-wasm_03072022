using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ToolsApp.Data;

public class DataContext
{
  private readonly string _connectionString;

  public DataContext(IConfiguration configuration) {
    _connectionString = configuration.GetConnectionString("App");
  }

  public DataContext() {
    _connectionString = "";
  }

  public virtual IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
