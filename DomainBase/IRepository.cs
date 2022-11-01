﻿namespace DomainBase
{
    public interface IRepository<T>
        where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
