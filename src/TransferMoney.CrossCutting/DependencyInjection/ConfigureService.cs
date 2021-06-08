using Microsoft.Extensions.DependencyInjection;
using TransferMoney.Domain.Interfaces;
using TransferMoney.Service.Services;

namespace TransferMoney.CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAccountInformationService, AccountInformationService>();
            serviceCollection.AddTransient<IFundTransferService, FundTransferService>();
            serviceCollection.AddTransient<ITransferMoneyProducerKafka, TransferMoneyKafkaProducer>();
        }
    }
}
