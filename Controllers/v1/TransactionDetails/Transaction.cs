using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;

namespace SlippageBackend.Controllers.v1.TransactionDetails
{
    [Route("api/[controller]")]
    [ApiController]
    public class Transaction : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTransactionDetails([FromQuery] string txHash)
        {
            var web3 = new Web3(Consts.Consts.RPC); // Use your Infura Project ID or your own Ethereum node

            var transactionId = txHash;

            var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionId);

            return Ok(transaction);
        }
    }
}
