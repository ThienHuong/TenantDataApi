using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolverApi.DataAccess;
using SolverApi.Models;
using SolverApi.Services;

namespace SolverApi.Controllers
{
    /*
        This controller focuses on table level
     */
    [ApiController]
    [Route("/databases/{databaseId}/data")] 
    public class DataController : ControllerBase
    {
        private readonly TenantDbContext _tenantDbContext;
        private readonly Tenant _tenant;
        string _tenantId = null;
        private readonly IDatabaseRepository _databaseRepository;
        //private readonly IMapper _mapper;

        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger, TenantDbContext tenantDbContext, IDatabaseRepository databaseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _tenantDbContext = tenantDbContext;
            if (httpContextAccessor.HttpContext != null)
            {
                _tenant = (Tenant)httpContextAccessor.HttpContext.Items["TENANT"];
                _databaseRepository = new DatabaseRepository(_tenantDbContext);
            }
        }

        [HttpGet("{tableName}")]   
        //[Authorize] //tenant would've have to be authenticated
        public IActionResult GetTableContent(int databaseId, string tablename)
        {
            //validate tenant
            _tenantDbContext.Database.EnsureCreated();

            if (_tenantDbContext == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "'tenantId' header value is missing");
            }

            //validate databaseId exists for this tenant
            if (_databaseRepository.GetDatabase(databaseId) == null)
            {
                return NotFound();
            }

            //validate table exists for this tenant and database
            var tableContent = _databaseRepository.GetTableContent(databaseId, tablename);

            if (tableContent == null)
            {
                return NotFound();
            }

            return Ok(tableContent);
        }
    }
}
