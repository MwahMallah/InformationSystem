using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace InformationSystem.DAL.Factories {
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InformationSystemDbContext>
    {
        private readonly DbContextSqliteFactory _dbContextSqliteFactory = new($"InformationSystem.db");
        public InformationSystemDbContext CreateDbContext(string[] args)
        {
            return _dbContextSqliteFactory.CreateDbContext();
        }
    }
}
