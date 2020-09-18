using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolverApi.Models;

namespace SolverApi.DataAccess
{
    public class DbSeeder
    {
        private readonly GlobalDbContext _context;

        public DbSeeder(GlobalDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            //separate db collection storing active tenants and their custom settings
            _context.Database.EnsureCreated();
            var tenants = new List<Tenant>()
            {
                new Tenant()
                {
                    Guid = new Guid("43ce6f06-a472-461f-b990-3a25c7f44b7a"),
                    Name = "TenantOne",
                    ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=MT_TenantOne;Trusted_Connection=true;MultipleActiveResultSets=true",
                    //collection of tables and custom labeling for columns
                    MappedTables = "{\"TenantTables\":[\"TableName\": \"TableOne\",\"ColumnLabels\":[{ \"Column\":\"Column2\", \"Label\":\"Column Two\"}]]}"
                },
                new Tenant()
                {
                    Guid = new Guid("199b625e-6ac6-4757-a38f-9a0391866469"),
                    Name = "TenantTwo",
                    ConnectionString = "Server=127.0.0.1, 1433;Database=WideWorldImporters;User Id=sa;Password=Password1;Trusted_Connection=True;MultipleActiveResultSets=true",
                    //collection of tables and custom labeling for columns
                    MappedTables = "{\"TenantTables\":[{\"TableName\": \"Application.Cities\",\"ColumnLabels\":[{ \"Column\":\"CityName\", \"Label\":\"Name of City\"},{\"Column\":\"LatestRecordedPopulation\", \"Label\":\"Current Population\"}]}]}"
                }
            };
            if (!_context.Tenants.Any())
            {
                _context.Tenants.AddRange(tenants);
                _context.SaveChanges();
            }
        }
    }
}
