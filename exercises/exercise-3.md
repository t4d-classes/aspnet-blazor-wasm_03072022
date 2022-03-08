# Exercise 3

1. Create a new implementation of the Cars data service and name it `CarsSqlServerData`.

1. In th new `CarsSqlServerData`, implement the code to get all cars and a single car from the database.

1. For the other methods, implement their statement block with the following code:

```csharp
throw new NotImplementedException
```

1. Add some configuration data (`appsettings.json`) and logic (Container Builder) to enable the switching between the In-Memory and SQL Server versions based upon configuration.

1. Implement the append, replace, and remove car operations in the `CarsSqlServerData`.

1. Ensure it works!