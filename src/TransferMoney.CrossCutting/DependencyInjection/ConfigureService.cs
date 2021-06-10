using Microsoft.Extensions.DependencyInjection;
using TransferMoney.Domain.Interfaces;
using TransferMoney.Service.Services;

namespace TransferMoney.CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAccountInformationService, AccountInformationService>();
            serviceCollection.AddScoped<IFundTransferService, FundTransferService>();
            serviceCollection.AddSingleton<ITransferMoneyProducerKafka, TransferMoneyKafkaProducer>();
        }
    }
}
