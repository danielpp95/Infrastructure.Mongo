namespace Infrastructure.Mongo;

using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        return services
            .AddSingleton<IMongoClient>(client)
            .AddSingleton(database)
            .AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));
    }
}
