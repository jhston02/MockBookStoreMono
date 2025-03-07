﻿using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb
{
    internal class NoOpUnitOfWork : IUnitOfWork
    {
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}