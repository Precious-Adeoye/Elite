using Elite.Application.DTOs;
using Elite.Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elite.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _Service;

        public TransactionController(ITransactionService service)
        {
            _Service = service;
        }

        [HttpPost("add-money")]
        public async Task<IActionResult> AddMoneyAsync([FromBody] TransactionDto dto)
        {
            try
            {
                await _Service.AddMoneyAsync(dto.UserId, dto.Amount, dto.Description);
                return Ok(new { message = "Money added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("send-money")]
        public async Task<IActionResult> SendMoney([FromBody] TransferDto dto)
        {
            try
            {
                await _Service.SendMoneyAsync(dto.senderId, dto.ReceiverAccountNumber, dto.Amount, dto.Description, dto.IsExternal, dto.BankCode);
                return Ok(new { message = "Money sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
