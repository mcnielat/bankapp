using BankApp.Controllers;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankAppTests
{
    /// <summary>
    /// Unit tests for TransactionsController
    /// </summary>
    public class TransactionsControllerTests
    {
        private readonly Mock<ITransactionsService> _transactionServiceMock;
        private readonly TransactionsController _transactionController;
        public TransactionsControllerTests()
        {
            _transactionServiceMock = new Mock<ITransactionsService>();
            _transactionController = new TransactionsController(_transactionServiceMock.Object);
        }

        [Fact]
        public async Task BalanceInquiry_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var accountId = 1;
            var hashedPin = "hashedPin";
            var hashedPassword = "hashedPassword";

            var expectedUserInfo = new UserDto
            {
                AccountId = 1,
                Balance = 0,
                UserName = "username"
            };

            _transactionServiceMock
                .Setup(x => x.InquireBalance(accountId, hashedPin, hashedPassword))
                .ReturnsAsync(expectedUserInfo);

            // Act
            var result = await _transactionController.BalanceInquiry(accountId, hashedPin, hashedPassword);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userInfo = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(expectedUserInfo, userInfo);
        }

        [Fact]
        public async Task BalanceInquiry_InvalidAccountId_ReturnsBadRequest()
        {
            // Arrange
            var accountId = 0;
            var hashedPin = "hashedPin";
            var hashedPassword = "hashedPassword";

            // Act
            var result = await _transactionController.BalanceInquiry(accountId, hashedPin, hashedPassword);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Account ID cannot be empty", badRequestResult.Value);
        }

        [Fact]
        public async Task BalanceInquiry_EmptyPinAndPassword_ReturnsBadRequest()
        {
            // Arrange
            var accountId = 1;
            string? hashedPin = null;
            string? hashedPassword = null;

            // Act
            var result = await _transactionController.BalanceInquiry(accountId, hashedPin, hashedPassword);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("PIN and password cannot be empty at the same time", badRequestResult.Value);
        }
    }
}