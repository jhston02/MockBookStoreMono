﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Shared;

namespace MabelBookshelf.Bookshelf.Application.Tests.Mocks
{
    public class MockExternalBookService : IExternalBookService
    {
        public Task<ExternalBook> GetBookAsync(string externalBookId, CancellationToken token = default)
        {
            if (externalBookId == "bad")
                throw new ArgumentException("bad");
            return Task.FromResult(new ExternalBook("blah", "blah", new[] { "test" }, "test", 90, new[] { "test" }));
        }
    }
}