using Banking.Transactions.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Banking.Framework.Domain;
using Banking.Framework.Services.Interface;
using Banking.Framework.Types;
using Banking.Transactions.Services;

namespace Banking.Transactions.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AccountController(ITransactionService transactionService, IIdentityService identityService, IMapper mapper)
        {
            _transactionService = transactionService;
            _identityService = identityService;
            _mapper = mapper;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> Balance()
        {
            var accountNumber = _identityService.GetIdentity().AccountNumber;
            var transactionResult = await _transactionService.Balance(accountNumber);
            return Ok(_mapper.Map<TransactionResultModel>(transactionResult));
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionModel accountTransactionModel)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionModel);
            accountTransaction.TransactionType = TransactionType.Deposit;
            var result = await _transactionService.Deposit(accountTransaction);
            return Created(string.Empty, _mapper.Map<TransactionResultModel>(result));
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionModel accountTransactionModel)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionModel);
            accountTransaction.TransactionType = TransactionType.Withdrawal;
            var result = await _transactionService.Withdraw(accountTransaction);
            return Created(string.Empty, _mapper.Map<TransactionResultModel>(result));
        }
    }
}
