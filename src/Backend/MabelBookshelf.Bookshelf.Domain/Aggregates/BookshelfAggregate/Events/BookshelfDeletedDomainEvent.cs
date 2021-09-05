﻿using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfDeletedDomainEvent : BookshelfDomainEvent
    {
        public BookshelfDeletedDomainEvent(Guid bookshelfId) : base(bookshelfId)
        {
        }
    }
}