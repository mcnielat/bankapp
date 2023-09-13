using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    /// <summary>
    /// Class that contains API endpoints for the bank transactions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]   
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        /// <summary>
        /// Endpoint for the balance inquiry action
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="hashedPin"></param>
        /// <param name="hashedPassword"></param>
        /// <returns>User information with the account balance</returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpGet]
        [Route("api/transactions/balanceinquiry/{accountId}")]
        public async Task<ActionResult> BalanceInquiry([FromRoute] int accountId,
                                                       string? hashedPin = "",
                                                       string? hashedPassword = "")
        {
            try
            {
                if (accountId <= 0)
                    throw new ArgumentException("Account ID cannot be empty");

                if (string.IsNullOrEmpty(hashedPin) && string.IsNullOrEmpty(hashedPassword))
                    throw new ArgumentException("PIN and password cannot be empty at the same time");

                var userInfo = await _transactionsService.InquireBalance(accountId, 
                                                                         hashedPin, 
                                                                         hashedPassword);

                return Ok(userInfo);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"BalanceInquiry encountered an error: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint for the withdrawal action
        /// </summary>
        /// <param name="withdrawalInfo"></param>
        /// <returns>Updated information of the user after the withdrawal</returns>
        [HttpPost]
        [Route("api/transactions/withdraw")]
        public async Task<ActionResult> Withdraw(Withdrawal withdrawalInfo)
        {
            try
            {
                withdrawalInfo.Validate();

                var updatedUserInfo = await _transactionsService.WithdrawMoney(withdrawalInfo);

                return Ok(updatedUserInfo);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Withdrawal encountered an error: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint for the deposit action
        /// </summary>
        /// <param name="depositInfo"></param>
        /// <returns>Updated information of the user after the deposit</returns>
        [HttpPost]
        [Route("api/transactions/deposit")]
        public async Task<ActionResult> Deposit(Deposit depositInfo)
        {
            try
            {
                depositInfo.Validate();

                var updatedUserInfo = await _transactionsService.DepositMoney(depositInfo);

                return Ok(updatedUserInfo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Withdrawal encountered an error: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint for the action to transfer money from one account to another
        /// </summary>
        /// <param name="transferMoneyInfo"></param>
        /// <returns>Updated information of the source account</returns>
        [HttpPost]
        [Route("api/transactions/transfer")]
        public async Task<ActionResult> Transfer(TransferMoney transferMoneyInfo)
        {
            try
            {
                transferMoneyInfo.Validate();

                var updatedUserInfo = await _transactionsService.TransferMoney(transferMoneyInfo);

                return Ok(updatedUserInfo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Withdrawal encountered an error: {ex.Message}");
            }
        }

    }
}
