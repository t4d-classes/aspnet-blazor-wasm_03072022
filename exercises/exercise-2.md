# Exercise 2

1. Upgrade the REST API to support the following operations:

- Append car
- Replace a car
- Remove a car

1. For each operation, use the usual REST API conventions.

// Append Car
- POST http://localhost:1234/Cars
- Body: New Car Object serialized to JSON

// Replace Car
- PUT http://localhost:1234/Cars/1
- Body: Car Object serialized to JSON

// Remove Car
- DELETE http://localhost:1234/Cars/1
- Body: None

1. Ensure it works!