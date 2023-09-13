using BankApp.Controllers;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankAppTests
{
    /// <summary>
    /// Unit tests for UserController
    /// </summary>
    public class UsersControllerTests
    {
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly UsersController _usersController;
        public UsersControllerTests()
        {
            _usersServiceMock = new Mock<IUsersService>();
            _usersController = new UsersController(_usersServiceMock.Object);
        }


        [Fact]
        public async Task AddUser_ValidInput_ReturnsActionResultUser()
        {
            // Arrange
            var hashedPin = "hashedPin";
            var hashedPassword = "hashedPassword";

            // Arrange
            var inputUser = new User
            {
                AccountId = 0,
                HashedPassword = hashedPassword,
                HashedPin = hashedPin,
            };

            var expectedUser = new User
            {
                AccountId = 1,
                HashedPassword = hashedPassword,
                HashedPin = hashedPin,
            };

            _usersServiceMock.Setup(s => s.AddUser(inputUser)).ReturnsAsync(expectedUser);

            // Act
            var result = await _usersController.AddUser(inputUser);

            // Assert
            var castedResult = (CreatedAtActionResult)result.Result;
            Assert.Equal("AddUser", castedResult.ActionName);
        }

        [Fact]
        public async Task AddUser_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var accountId = 0;
            var hashedPin = "";
            var hashedPassword = "";

            // Arrange
            var user = new User
            {
                AccountId = accountId,
                HashedPassword = hashedPassword,
                HashedPin = hashedPin,
            };

            _usersServiceMock.Setup(s => s.AddUser(user)).Throws<ArgumentException>();


            // Act
            var result = await _usersController.AddUser(user);

            // Assert
            var castedResult = (BadRequestObjectResult)result.Result;
            Assert.Equal("PIN and password cannot be empty at the same time.", castedResult.Value);

        }

        [Fact]
        public async Task AddUser_ServiceError_ReturnsInternalServerError()
        {
            // Arrange
            var accountId = 0;
            var hashedPin = "hashedPin";
            var hashedPassword = "hashedPassword";

            // Arrange
            var user = new User
            {
                AccountId = accountId,
                HashedPassword = hashedPassword,
                HashedPin = hashedPin,
            };

            _usersServiceMock.Setup(s => s.AddUser(user)).Throws<Exception>();

            // Act
            var result = await _usersController.AddUser(user);

            // Assert
            var internalServerErrorResult = (ObjectResult)result.Result;
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
        }

    }
}