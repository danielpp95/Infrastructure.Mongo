namespace Infrastructure.Mongo;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

internal class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> collection;

    public MongoRepository(IMongoDatabase mongoDb)
    {
        MapClasses();

        this.collection = mongoDb.GetCollection<T>(typeof(T).Name);
    }

    private static void MapClasses()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
            BsonClassMap.RegisterClassMap<T>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }

    public async Task<T> FindByIdAsync(Guid id, CancellationToken token = default)
    {
        var cursor = await this.collection.FindAsync(x => x.Id == id);

        return await cursor.SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken token = default)
    {
        var cursor = await this.collection.FindAsync(expression);

        return await cursor.ToListAsync();
    }

    public async Task SaveAsync(T entity, CancellationToken cancellationToken = default)
    {
        await this.collection.ReplaceOneAsync(
            x => x.Id.Equals(entity.Id),
            entity,
            new ReplaceOptions
            {
                IsUpsert = true,
            },
            cancellationToken);
    }

    public async Task SaveAsync(IReadOnlyCollection<T> entities, CancellationToken cancellationToken = default)
    {
        if (!entities?.Any() ?? true)
        {
            return;
        }

        var operations = entities!.Select(
            entity => new ReplaceOneModel<T>(
                new ExpressionFilterDefinition<T>(x => x.Id.Equals(entity.Id)),
                entity)
            {
                IsUpsert = true,
            });

        await this.collection.BulkWriteAsync(operations, cancellationToken: cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return this.collection.DeleteOneAsync(x => x.Id.Equals(id), cancellationToken);
    }
}
