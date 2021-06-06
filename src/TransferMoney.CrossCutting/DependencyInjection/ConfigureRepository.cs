using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using TransferMoney.Data.Context;

namespace TransferMoney.CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<TransferMoneyDbContext>(
                   options =>
                   options.UseMySql("Persist Security Info=True;Server=localhost;Port=3306;Database=dbMarketApi;Uid=root;Pwd=985206",
                           new MySqlServerVersion(new Version(8, 0, 21)),
                           mySqlOptions => mySqlOptions
                           .CharSetBehavior(CharSetBehavior.NeverAppend))
               );
        }
    }
}
