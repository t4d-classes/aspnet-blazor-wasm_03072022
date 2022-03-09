# Exercise 1

### Note: Please use existing projects and create a new folders as needed to organize code within the projects.

1. Add a new In-Memory Cars data service named `CarsInMemoryData`.

- Car Model Properties
- Id: int
- Make: string
- Model: string
- Year: int
- Color: string
- Price: decimal

1. Register the service with Autofac.

1. Create a new `CarsController` to handle request for getting all cars and getting a single car by id.

1. Utilize the `CarsInMemoryData` to satisfy the data needed for the `CarsController` to fulfill its requests.

1. Use the `Task` API to wrap all data service methods and controller actions.

1. Ensure it works!