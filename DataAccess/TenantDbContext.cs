using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SolverApi.Models;
using System.Collections.Generic;
using System.Data;

namespace SolverApi.DataAccess
{
    public class TenantDbContext : DbContext
    {
        private readonly Tenant _tenant;


        public TenantDbContext(DbContextOptions<TenantDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            if (httpContextAccessor.HttpContext != null)
            {
                _tenant = (Tenant)httpContextAccessor.HttpContext.Items["TENANT"];
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_tenant.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        //note:need to add logic to check what type of database used, ex mysql/sqlite/mssql

        public DataTable GetListofDatabases()
        {
            string query = "select name, database_id from master.sys.databases";
            return GetByQuery(query);
        }

        public DataTable GetListofTables(string databaseName)
        {
            string query = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '{databaseName}'"; //TABLE_SCHEMA for mysql
            return GetByQuery(query);
        }

        public DataTable GetTableContent(string dbName, string tableName)
        {
            string query = $"use {dbName}; select * from {tableName}";
            return GetByQuery(query);
        }

        public DataTable GetByQuery(string query)
        {
            using (var sqlConnection = new SqlConnection(_tenant.ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        return tb;
                    }
                }
            }
        }
    }
}
