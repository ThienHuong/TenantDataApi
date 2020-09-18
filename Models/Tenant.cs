using System;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace SolverApi.Models
{
    public class Tenant
    {
        [Key]
        public Guid Guid { get; set; }

        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string MappedTables { get; set; }
    }
}
