using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using TransferMoney.Data.Context;
using TransferMoney.Data.Repository;
using TransferMoney.Domain.Interfaces;
using TransferMoney.Domain.Interfaces.Repository;
using TransferMoney.Service.Services;

namespace TranferMoney.KafkaConsumerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddHostedService<TransferMoneyConsumerKafka>();
                    collection.AddTransient<IAccountInformationService, AccountInformationService>();
                    collection.AddScoped<ITransferMoneyRepository, TransferMoneyRepository>();
                    collection.AddDbContext<TransferMoneyDbContext>(
                        options =>
                        options.UseMySql("Persist Security Info=True;Server=localhost;Port=3306;Database=TransferMoneyDb;Uid=root;Pwd=985206",
                        new MySqlServerVersion(new Version(8, 0, 21)),
                        mySqlOptions => mySqlOptions
                        .CharSetBehavior(CharSetBehavior.NeverAppend)));
                });
    }
}
