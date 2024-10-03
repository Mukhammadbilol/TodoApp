using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Test.ControllerTests;

public class TodoItemsControllerTests
{
    [Fact]
    public async Task GetTodos_ReturnsAllItems()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            context.Todos.AddRange(
                new TodoItem { Id = 1, Title = "Task 1", Description = "Task 1 description" },
                new TodoItem { Id = 2, Title = "Task 2", Description = "Task 2 description" }
            );
            context.SaveChanges();
        }

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var result = await controller.GetTodos();

            var okResult = Assert.IsType<ActionResult<IEnumerable<TodoItem>>>(result);
            var returnValue = Assert.IsType<List<TodoItem>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }

    [Fact]
    public async Task GetTodoItem_ReturnsItem_IfFound()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_GetTodoItem")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            context.Todos.Add(new TodoItem { Id = 1, Title = "Task 1", Description = "Task 1 description" });
            context.SaveChanges();
        }

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var result = await controller.GetTodoItem(1);

            var okResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var returnValue = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Task 1", returnValue.Title);
        }
    }

    [Fact]
    public async Task GetTodoItem_ReturnsNotFound_IfNotExists()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_GetTodoItem_NotFound")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var result = await controller.GetTodoItem(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

    [Fact]
    public async Task PostTodoItem_AddsNewItemToDatabase()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_PostTodoItem")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var newTodoItem = new TodoItem { Title = "New Task", Description = "New Task description" };

            var result = await controller.PostTodoItem(newTodoItem);

            var todoItems = await context.Todos.ToListAsync();
            Assert.Single(todoItems);
            Assert.Equal("New Task", todoItems[0].Title);
            Assert.Equal("New Task description", todoItems[0].Description);
        }
    }

    [Fact]
    public async Task PostTodoItem_ReturnsCreatedAtActionResult()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_PostTodoItem_CreatedAt")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var newTodoItem = new TodoItem { Title = "New Task", Description = "New Task description" };

            var result = await controller.PostTodoItem(newTodoItem);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);

            Assert.Equal("New Task", returnValue.Title);
            Assert.Equal("New Task description", returnValue.Description);
            Assert.Equal(1, returnValue.Id);
        }
    }

    [Fact]
    public async Task PutTodoItem_UpdatesExistingItem_ReturnsNoContent()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_PutTodoItem_Updates")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            context.Todos.Add(new TodoItem { Id = 1, Title = "Task 1", Description = "Task 1 description" });
            context.SaveChanges();
        }

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var updatedTodoItem = new TodoItem { Id = 1, Title = "Updated Task 1", Description = "Updated description" };

            var result = await controller.PutTodoItem(1, updatedTodoItem);

            Assert.IsType<NoContentResult>(result);

            var todoItem = await context.Todos.FindAsync(1);
            Assert.Equal("Updated Task 1", todoItem!.Title);
            Assert.Equal("Updated description", todoItem.Description);
        }
    }

    [Fact]
    public async Task PutTodoItem_ReturnsBadRequest_IfIdMismatch()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_PutTodoItem_BadRequest")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var todoItem = new TodoItem { Id = 1, Title = "Task 1", Description = "Task 1 description" };

            var result = await controller.PutTodoItem(2, todoItem);

            Assert.IsType<BadRequestResult>(result);
        }
    }

    [Fact]
    public async Task PutTodoItem_ReturnsNotFound_IfItemDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_PutTodoItem_NotFound")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var nonExistentTodoItem = new TodoItem { Id = 999, Title = "Nonexistent Task", Description = "Nonexistent description" };

            var result = await controller.PutTodoItem(999, nonExistentTodoItem);

            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task DeleteTodoItem_RemovesItem_ReturnsNoContent()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_DeleteTodoItem_Removes")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            context.Todos.Add(new TodoItem { Id = 1, Title = "Task 1", Description = "Task 1 description" });
            context.SaveChanges();
        }

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var result = await controller.DeleteTodoItem(1);

            Assert.IsType<NoContentResult>(result);

            var todoItem = await context.Todos.FindAsync(1);
            Assert.Null(todoItem);
        }
    }

    [Fact]
    public async Task DeleteTodoItem_ItemDoesNotExist_ReturnsNotFound()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: "TodoListDatabase_DeleteTodoItem_NotFound")
            .Options;

        using (var context = new TodoDbContext(options))
        {
            var controller = new TodoItemsController(context);

            var result = await controller.DeleteTodoItem(999);
            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
