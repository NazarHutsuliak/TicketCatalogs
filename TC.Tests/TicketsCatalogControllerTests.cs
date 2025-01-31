using Moq;
using Microsoft.AspNetCore.Mvc;
using TC.API.Controllers;
using TC.Application.Interfaces;
using TC.Application.Entities;
using Microsoft.Extensions.Logging;

[TestFixture]
public class TicketsCatalogControllerTests
{
    private Mock<ITicketsCatalogService> _mockService;
    private TicketsCatalogController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockService = new Mock<ITicketsCatalogService>();
        _controller = new TicketsCatalogController(Mock.Of<ILogger<TicketsCatalogController>>(), _mockService.Object);
    }

    [Test]
    public async Task GetCatalogById_ReturnsOk_WhenCatalogExists()
    {
        // Arrange
        var catalogId = 1;
        var userId = 123;
        var expectedCatalog = new TicketsCatalog
        {
            Id = catalogId,
            Name = "Test Catalog",
            DataTime = DateTime.Now,
            Owner = new User { Id = 1, Name = "Owner Name" },
            Users = new List<User> { new User { Id = 2, Name = "User 1" } },
            Tasks = new List<Ticket> { new Ticket { Id = 1, Description = "Task 1" } }
        };

        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId))
                    .ReturnsAsync(expectedCatalog);

        // Act
        var result = await _controller.GetCatalogById(catalogId, userId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedCatalog, okResult.Value);
    }

    [Test]
    public async Task GetCatalogById_ReturnsNotFound_WhenCatalogDoesNotExist()
    {
        // Arrange
        var catalogId = 1;
        var userId = 123;
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId))
                    .ReturnsAsync((TicketsCatalog)null);

        // Act
        var result = await _controller.GetCatalogById(catalogId, userId);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task GetAllCatalogs_ReturnsOk_WhenCatalogsExist()
    {
        // Arrange
        var userId = 123;
        var catalogs = new List<TicketsCatalog>
            {
                new TicketsCatalog {
                    Id = 1,
                    Name = "Catalog 1",
                    DataTime = DateTime.Now,
                    Owner = new User { Id = 1, Name = "Owner 1" },
                    Users = new List<User> { new User { Id = 2, Name = "User 1" } },
                    Tasks = new List<Ticket> { new Ticket { Id = 1, Description = "Task 1" } }
                },
                new TicketsCatalog {
                    Id = 2,
                    Name = "Catalog 2",
                    DataTime = DateTime.Now,
                    Owner = new User { Id = 2, Name = "Owner 2" },
                    Users = new List<User> { new User { Id = 3, Name = "User 2" } },
                    Tasks = new List<Ticket> { new Ticket { Id = 2, Description = "Task 2" } }
                }
            };
        _mockService.Setup(s => s.GetAllCatalogsAsync(userId))
                    .ReturnsAsync(catalogs);

        // Act
        var response = await _controller.GetAllCatalogs(userId, 1, 10);

        // Assert
        var okResult = response.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [Test]
    public async Task GetAllCatalogs_ReturnsNotFound_WhenNoCatalogsExist()
    {
        // Arrange
        var userId = 123;
        _mockService.Setup(s => s.GetAllCatalogsAsync(userId))
                    .ReturnsAsync(new List<TicketsCatalog>());

        // Act
        var result = await _controller.GetAllCatalogs(userId, 1, 10);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task CreateCatalog_ReturnsCreated_WhenValidCatalogProvided()
    {
        // Arrange
        var userId = 123;
        var catalog = new TicketsCatalog
        {
            Id = 1,
            Name = "New Catalog",
            DataTime = DateTime.Now,
            Owner = new User { Id = 1, Name = "Owner Name" },
            Users = new List<User> { new User { Id = 2, Name = "User 1" } },
            Tasks = new List<Ticket> { new Ticket { Id = 1, Description = "Task 1" } }
        };
        _mockService.Setup(s => s.CreateCatalogAsync(catalog, userId));

        // Act
        var result = await _controller.CreateCatalog(catalog, userId);

        // Assert
        var createdAtActionResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdAtActionResult);
        Assert.AreEqual(201, createdAtActionResult.StatusCode);
        Assert.AreEqual(catalog, createdAtActionResult.Value);
    }

    [Test]
    public async Task CreateCatalog_ReturnsBadRequest_WhenCatalogIsNull()
    {
        // Arrange
        var userId = 123;
        TicketsCatalog catalog = null;

        // Act
        var result = await _controller.CreateCatalog(catalog, userId);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [Test]
    public async Task UpdateCatalog_ReturnsNoContent_WhenCatalogUpdated()
    {
        // Arrange
        var catalogId = 1;
        var userId = 123;
        var catalogToUpdate = new TicketsCatalog
        {
            Id = catalogId,
            Name = "Updated Catalog",
            DataTime = DateTime.Now,
            Owner = new User { Id = 1, Name = "Owner Name" },
            Users = new List<User> { new User { Id = 2, Name = "User 1" } },
            Tasks = new List<Ticket> { new Ticket { Id = 1, Description = "Task 1" } }
        };
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId))
                    .ReturnsAsync(new TicketsCatalog { Id = catalogId, Name = "Old Catalog",
                        Owner = new User { Id = 2, Name = "User 1" }, DataTime = DateTime.Now });
        _mockService.Setup(s => s.UpdateCatalogAsync(catalogToUpdate, userId));

        // Act
        var result = await _controller.UpdateCatalog(catalogId, catalogToUpdate, userId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task UpdateCatalog_ReturnsNotFound_WhenCatalogDoesNotExist()
    {
        // Arrange
        var catalogId = 1;
        var userId = 123;
        var catalogToUpdate = new TicketsCatalog { Id = catalogId, Name = "Updated Catalog", 
            Owner = new User { Id = 2, Name = "User 1" }, DataTime = DateTime.Now };
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId))
                    .ReturnsAsync((TicketsCatalog)null);

        // Act
        var result = await _controller.UpdateCatalog(catalogId, catalogToUpdate, userId);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task DeleteCatalog_ReturnsNoContent_WhenCatalogDeleted()
    {
        // Arrange
        var catalogId = 1;
        var userId = 123;
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId))
                    .ReturnsAsync(new TicketsCatalog { Id = catalogId, Name = "Catalog to Delete",
                        Owner = new User { Id = 2, Name = "User 1" },
                        DataTime = DateTime.Now });
        _mockService.Setup(s => s.DeleteCatalogAsync(catalogId, userId));

        // Act
        var result = await _controller.DeleteCatalog(catalogId, userId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task DeleteCatalog_ReturnsNotFound_WhenCatalogDoesNotExist()
    {
        // Arrange
        var catalogId = 1;
        var userId = 123;
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId))
                    .ReturnsAsync((TicketsCatalog)null);

        // Act
        var result = await _controller.DeleteCatalog(catalogId, userId);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task AddUserToCatalog_ShouldReturnBadRequest_WhenUserIsNull()
    {
        // Arrange
        var catalogId = 1;
        var userId = 1;
        User userToAdd = null;

        // Act
        var result = await _controller.AddUserToCatalog(catalogId, userId, userToAdd);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task AddUserToCatalog_ShouldReturnNotFound_WhenCatalogIsNotFound()
    {
        // Arrange
        var userId = 123;
        var catalog = new TicketsCatalog
        {
            Id = 1,
            Name = "New Catalog",
            DataTime = DateTime.Now,
            Owner = new User { Id = 1, Name = "Owner Name" },
            Users = new List<User> { new User { Id = 2, Name = "User 1" } },
            Tasks = new List<Ticket> { new Ticket { Id = 1, Description = "Task 1" } }
        };
        _mockService.Setup(s => s.CreateCatalogAsync(catalog, userId));

        // Act
        var result = await _controller.AddUserToCatalog(2, userId, new User { Id = 3, Name = "User 1" });

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }

    [Test]
    public async Task AddUserToCatalog_ShouldReturnNoContent_WhenUserIsAddedSuccessfully()
    {
        // Arrange
        var catalogId = 1;
        var userId = 1;
        var userToAdd = new User { Id = 2, Name = "John Doe" };
        var catalog = new TicketsCatalog { Id = catalogId, Name = "Test Catalog", Owner = new User { Id = userId, Name = "Owner" } };
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId)).ReturnsAsync(catalog);
        _mockService.Setup(s => s.AddUserToCatalogAsync(catalogId, userId, userToAdd)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddUserToCatalog(catalogId, userId, userToAdd);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task DeleteUserFromCatalog_ShouldReturnNotFound_WhenCatalogIsNotFound()
    {
        // Arrange
        var catalog = new TicketsCatalog
        {
            Id = 1,
            Name = "New Catalog",
            DataTime = DateTime.Now,
            Owner = new User { Id = 1, Name = "Owner Name" },
            Users = new List<User> { new User { Id = 2, Name = "User 1" } },
            Tasks = new List<Ticket> { new Ticket { Id = 1, Description = "Task 1" } }
        };
        _mockService.Setup(s => s.CreateCatalogAsync(catalog, 1));

        // Act
        var result = await _controller.DeleteUserFromCatalog(2, 1, 2);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }

    [Test]
    public async Task DeleteUserFromCatalog_ShouldReturnNoContent_WhenUserIsDeletedSuccessfully()
    {
        // Arrange
        var catalogId = 1;
        var userId = 1;
        var userToRemoveId = 2;
        var catalog = new TicketsCatalog { Id = catalogId, Name = "Test Catalog", Owner = new User { Id = userId, Name = "Owner" } };
        _mockService.Setup(s => s.GetCatalogByIdAsync(catalogId, userId)).ReturnsAsync(catalog);
        _mockService.Setup(s => s.DeleteUserFromCatalogAsync(catalogId, userId, userToRemoveId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUserFromCatalog(catalogId, userId, userToRemoveId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task GetUsersByCatalogId_ShouldReturnOk_WhenUsersAreFound()
    {
        // Arrange
        var catalogId = 1;
        var userId = 1;
        var users = new List<User>
        {
            new User { Id = 1, Name = "User 1" },
            new User { Id = 2, Name = "User 2" }
        };

        _mockService.Setup(s => s.GetAllUsersByCatalogIdAsync(catalogId, userId)).ReturnsAsync(users);

        // Act
        var result = await _controller.GetUsersByCatalogId(catalogId, userId);
        var okResult = result.Result as OkObjectResult;

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(okResult);
        Assert.AreEqual(users, okResult.Value);
    }

    [Test]
    public async Task GetUsersByCatalogId_ShouldReturnNotFound_WhenNoUsersAreFound()
    {
        // Arrange
        var catalogId = 1;
        var userId = 1;
        var users = new List<User>();

        _mockService.Setup(s => s.GetAllUsersByCatalogIdAsync(catalogId, userId)).ReturnsAsync(users);

        // Act
        var result = await _controller.GetUsersByCatalogId(catalogId, userId);
        var notFoundResult = result.Result as NotFoundObjectResult;

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(notFoundResult);
    }
}
