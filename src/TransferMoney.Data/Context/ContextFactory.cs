using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace TransferMoney.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<TransferMoneyDbContext>
    {
        public TransferMoneyDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Persist Security Info=True;Server=localhost;Port=3306;Database=TransferMoneyDb;Uid=root;Pwd=985206";
            var optionsBuilder = new DbContextOptionsBuilder<TransferMoneyDbContext>();
            optionsBuilder.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 21)),
                         mySqlOptions => mySqlOptions
                        .CharSetBehavior(CharSetBehavior.NeverAppend));
            return new TransferMoneyDbContext(optionsBuilder.Options);
        }
    }
}
