using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OrderManagement_BackEnd.DAL;


namespace OrderManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoneBookController : ControllerBase
    {
        private readonly DapperContext _context;

        public PoneBookController(DapperContext context)
        {
            _context = context;
        }

        [HttpGet("Companyms")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompany()
        {
            try
            {
                using var connection = _context.CreateConnection();
                var query = "EXEC GetAllCompanies";
                var company = await connection.QueryAsync(query);

                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Floorms")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                using var connection = _context.CreateConnection();
                var query = "EXEC GetAllFloor";
                var floor = await connection.QueryAsync(query);

                return Ok(floor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("CostCenterms")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCostCenter([FromQuery] string comcode)
        {
            try
            {
                using var connection = _context.CreateConnection();

                string query = string.Empty;
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(comcode))
                {
                    query = "EXEC sp_GetDepartmentBycomCode @COMPANY_CODE";
                    parameters.Add("COMPANY_CODE", comcode);
                }
                else
                {
                    query = "EXEC sp_GetDepartmentBycomCode";
                }
                var result = await connection.QueryAsync(query, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("PhoneBookms")]
        [ProducesResponseType(typeof(IEnumerable<dynamic>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPhoneBook([FromQuery] string keyword, [FromQuery] string floor, [FromQuery] string company, [FromQuery] string cc)
        {
            try
            {
                using var connection = _context.CreateConnection();

                string query = "EXEC sp_GetEmployeesByFilters";
                var parameters = new DynamicParameters();
                bool firstParamAdded = false;

                if (!string.IsNullOrEmpty(keyword))
                {
                    if (firstParamAdded)
                    {
                        query += ", @Keyword = @keyword";
                    }
                    else
                    {
                        query += " @Keyword = @keyword";
                        firstParamAdded = true;
                    }
                    parameters.Add("keyword", keyword);
                }

                if (!string.IsNullOrEmpty(floor))
                {
                    if (firstParamAdded)
                    {
                        query += ", @FLOOR = @floor";
                    }
                    else
                    {
                        query += " @FLOOR = @floor";
                        firstParamAdded = true;
                    }
                    parameters.Add("floor", floor);
                }

                if (!string.IsNullOrEmpty(company))
                {
                    if (firstParamAdded)
                    {
                        query += ", @COMPANY_CODE = @company_code";
                    }
                    else
                    {
                        query += " @COMPANY_CODE = @company_code";
                        firstParamAdded = true;
                    }
                    parameters.Add("company_code", company);
                }

                if (!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(cc))
                {
                    if (firstParamAdded)
                    {
                        query += ", @DEPARTMENT = @department";
                    }
                    else
                    {
                        query += " @DEPARTMENT = @department";
                        firstParamAdded = true;
                    }
                    parameters.Add("department", cc);
                }

                var result = await connection.QueryAsync(query, parameters);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
