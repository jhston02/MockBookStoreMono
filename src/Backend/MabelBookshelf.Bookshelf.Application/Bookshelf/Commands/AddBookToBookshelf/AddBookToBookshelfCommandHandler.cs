﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommandHandler : IRequestHandler<AddBookToBookshelfCommand, bool>
    {
        private readonly IBookshelfRepository _repository;

        public AddBookToBookshelfCommandHandler(IBookshelfRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(AddBookToBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf = await _repository.GetAsync(request.ShelfId, token: cancellationToken) ??
                            throw new ArgumentException("Bookshelf does not exist");
            bookshelf.AddBook(request.BookId);
            await _repository.UpdateAsync(bookshelf, cancellationToken);

            await _repository.UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}