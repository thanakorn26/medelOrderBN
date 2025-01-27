using Dapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement_BackEnd.DAL;

namespace OrderManagement_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DapperContext _context;

        public OrderController(DapperContext context)
        {
            _context = context;
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                using var connection = _context.CreateConnection();
                var query = "EXEC GetOrders";

                var orders = await connection.QueryAsync(query);

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    [HttpPost("AddOrder")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrder([FromBody] Order Order)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@ProductName", Order.ProductName);
                parameters.Add("@Quantity", Order.Quantity);
                parameters.Add("@Price", Order.Price);
                parameters.Add("@CustomerName", Order.CustomerName);
                parameters.Add("@CustomerEmail", Order.CustomerEmail);

                var newOrderId = await connection.ExecuteScalarAsync<int>(
                    "AddOrder",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure
                );
                return new JsonResult(new { Message = "Create Order Success"});

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
