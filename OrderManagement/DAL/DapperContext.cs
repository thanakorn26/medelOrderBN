﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace OrderManagement_BackEnd.DAL
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        // Constructor รับ IConfiguration เพื่อดึง Connection String จาก appsettings.json
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
