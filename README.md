
# Todo List API

This is a simple Todo List API built using ASP.NET Core and Entity Framework (EF) with MySQL. The API allows users to manage their todo items by creating, reading, updating, and deleting tasks, where each task has a title and description.

## Features

- **Create Todo Item**: Add a new todo item to the list.
- **Retrieve Todo Items**: Get a list of all todo items or retrieve a specific todo item by its ID.
- **Update Todo Item**: Update the title or description of an existing todo item.
- **Delete Todo Item**: Remove a todo item by its ID.

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: MySQL (with Entity Framework Core for data access)
- **Unit Testing**: XUnit
- **In-memory Database**: Entity Framework Core InMemory (for testing)

## Prerequisites

- **.NET 6 SDK or later**
- **MySQL** installed and running
- **Postman** or any API client (optional, for testing API endpoints)

## Setup Instructions

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/todo-list-api.git
   cd todo-list-api

2. **Update MySQL Configuration**  
- Open the `appsettings.json` file and update the connection string to match your MySQL database setup:

    ```json
    {
    "ConnectionStrings": {
        "TodoDbContext": "Server=localhost;Database=TodoDb;User Id=yourusername;Password=yourpassword;"
    },
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*"
    }

3. **Create the Database**  
- Before running the application, ensure the database is created. Run the following command to apply migrations and create the `TodoDb` database:

   ```bash
   dotnet ef database update

4. **Run the application**
- Run the application using the following command:
    ```bash
    dotnet run

- The application will start and be available at `https://localhost:5001` (or the port specified in `launchSettings.json`)

5. **Test API Endpoints**  
   You can test the following API endpoints using Postman or any other API testing tool:

   1. **Get All Todos**  
      **GET** `/api/todoitems`  
      Example: `https://localhost:5001/api/todoitems`

   2. **Get Todo by ID**  
      **GET** `/api/todoitems/{id}`  
      Example: `https://localhost:5001/api/todoitems/1`

   3. **Create a New Todo**  
      **POST** `/api/todoitems`  
      Example Body:
      ```json
      {
        "title": "New Task",
        "description": "This is a new task."
      }
      ```

   4. **Update an Existing Todo**  
      **PUT** `/api/todoitems/{id}`  
      Example Body:
      ```json
      {
        "id": 1,
        "title": "Updated Task",
        "description": "Updated description."
      }
      ```

   5. **Delete a Todo**  
      **DELETE** `/api/todoitems/{id}`  
      Example: `https://localhost:5001/api/todoitems/1`

6. **Running Unit Tests**  
   This project includes unit tests written using **XUnit** and **InMemoryDatabase**.

    - To run the unit tests, execute the following command in the project directory:
        ```bash
        dotnet test
    - All tests will be executed, including tests for the Todo API functionality, using an in-memory database for data isolation.

## Contributing  
If you'd like to contribute to this project, feel free to open a pull request or create an issue with suggestions for improvements.