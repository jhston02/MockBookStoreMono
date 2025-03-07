﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Tests.Mocks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate;
using Xunit;

namespace MabelBookshelf.Bookshelf.Application.Tests
{
    public class AddBookToBookshelfCommandValidatorTests
    {
        [Fact]
        public async Task ValidAddBookToBookshelfCommand_IsValid()
        {
            var id = Guid.NewGuid();
            var owner = "test";
            var bookId = "hey";
            var bookRepository = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>
            {
                new(bookId, owner,
                    await VolumeInfo.FromExternalId("blah", new MockExternalBookService()))
            });
            var bookshelfRepository =
                new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>
                    { new(id, "test", "test") });
            var validator = new AddBookToBookshelfCommandValidator(bookRepository, bookshelfRepository);
            var command = new AddBookToBookshelfCommand(bookId, id);
            Assert.True((await validator.ValidateAsync(command)).IsValid);
        }

        [Fact]
        public async Task InvalidCommand_BookDoesNotExist_Invalid()
        {
            var id = Guid.NewGuid();
            var bookId = "hey";
            var bookRepository = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>());
            var bookshelfRepository =
                new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>
                    { new(id, "test", "test") });
            var validator = new AddBookToBookshelfCommandValidator(bookRepository, bookshelfRepository);
            var command = new AddBookToBookshelfCommand(bookId, id);
            Assert.False((await validator.ValidateAsync(command)).IsValid);
        }

        [Fact]
        public async Task InvalidCommand_BookshelfDoesNotExist_Invalid()
        {
            var id = Guid.NewGuid();
            var owner = "test";
            var bookId = "hey";
            var bookRepository = new MockBookRepository(new List<Domain.Aggregates.BookAggregate.Book>
            {
                new(bookId, owner,
                    await VolumeInfo.FromExternalId("blah", new MockExternalBookService()))
            });
            var bookshelfRepository =
                new MockBookshelfRepository(new List<Domain.Aggregates.BookshelfAggregate.Bookshelf>());
            var validator = new AddBookToBookshelfCommandValidator(bookRepository, bookshelfRepository);
            var command = new AddBookToBookshelfCommand(bookId, id);
            Assert.False((await validator.ValidateAsync(command)).IsValid);
        }
    }
}