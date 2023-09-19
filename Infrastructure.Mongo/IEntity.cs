namespace Infrastructure.Mongo;

using System;

public interface IEntity
{
    Guid Id { get; }
}
