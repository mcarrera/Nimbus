# Nimbus
Nimbus is a RESTful API built with .NET 8 to manage and interact with a file system. 
It provides endpoints for handling file storage, organization, and retrieval, enabling users to easily interact with files and folders programmatically.

## Technologies
- **.NET 8**: The latest stable version of .NET, providing a robust platform for building APIs.
- **Entity Framework**: ORM for data access and manipulation.
- **Swagger**: API documentation and testing interface for easy exploration of available endpoints.

## How to run and Test the application
### Prerequisites
- .NET Core SDK 8
- Git
- A development IDE such as Visual Studio 2022 (optional)

### Steps

Clone the repository

```bash
  git clone https://github.com/mcarrera/Nimbus.git
```

Browse to the WebApi project directory
```bash
  cd src\Nimbus\Nimbus.WebApi
```

Build with dotnet
```bash
  dotnet build
```

Start the API

```bash
  dotnet run --urls=http://localhost:5027
```
Alternatively, you can use an IDE such as Visual Studio, or JetBrains Rider to open and run the Nimbus.sln solution.
To test the API, navigate to the Swagger UI Page (http://localhost:5027/swagger) where you can explore the available endpoints.
Notice the endpoint are secured with an API-KEY which should be provided separetely.

### Troubleshooting
If you encounter any issues while running the API:
* Check the console output for error messages.
* Ensure that the .NET 8 SDK is correctly installed.
* Make sure the specified port (5027) is not being used by another service.