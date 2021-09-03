﻿namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookDeletedDomainEvent : BookDomainEvent
    {
        public BookDeletedDomainEvent(string bookId) : base(bookId)
        {
        }
    }
}