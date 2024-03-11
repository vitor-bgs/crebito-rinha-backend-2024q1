using Crebito.Domain;
using Crebito.Domain.Queries;
using Crebito.Domain.Repository;
using Crebito.Infra.DataAccess.WithDapper;
using Crebito.Infra.DataAccess.WithDapper.Queries;
using Crebito.Infra.DataAccess.WithDapper.Repository;

namespace Crebito.Api.DependencyInjection
{
    public static class CrebitoDataAccessDepencency
    {
        public static void AddDapperRepository(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddNpgsqlDataSource(configuration.GetConnectionString("CrebitoDbConnection"));

            services.AddScoped<IUnitOfWork, UnitOfWorkDapper>();
            services.AddScoped<IContaRepository, ContaRepositoryDapper>();
            services.AddScoped<IObterExtratoQueryService, ObterExtratoQueryServiceDapper>();
        }
    }
}
