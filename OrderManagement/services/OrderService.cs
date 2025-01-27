using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class OrderService
{
    private readonly string _connectionString;

    public OrderService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        using (IDbConnection dbConnection = new SqlConnection(_connectionString))
        {
            return await dbConnection.QueryAsync<Order>("GetOrders", commandType: CommandType.StoredProcedure);
        }
    }

    public async Task AddOrderAsync(Order order)
    {
        using (IDbConnection dbConnection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", order.ProductName);
            parameters.Add("@Quantity", order.Quantity);
            parameters.Add("@Price", order.Price);
            parameters.Add("@CustomerName", order.CustomerName);
            parameters.Add("@CustomerEmail", order.CustomerEmail);

            await dbConnection.ExecuteAsync("AddOrder", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
