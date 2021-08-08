﻿using System;
using MediatR;
using MockBookStore.Bookshelf.Domain.Aggregates.BookAggregate.Events;
using MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookAggregate
{
    public class Book : Entity<Guid>
    {
        public string Title { get; private set; }
        public string[] Authors { get; private set; }
        public string ISBN  { get; private set; }
        public string ExternalId { get; private set; }
        public BookStatus Status { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentPageNumber { get; private set; }

        public Book(Guid id, string title, string[] authors, string isbn, string externalId, int totalPages, BookStatus status)
        {
            this.Id = id;
            this.Title = title;
            this.Authors = authors;
            this.ISBN = isbn;
            this.ExternalId = externalId;
            this.TotalPages = totalPages;
            this.Status = status;

            if (this.Status == BookStatus.Finish)
                CurrentPageNumber = totalPages;
            else
                CurrentPageNumber = 0;
        }

        public void StartReading()
        {
            if (Status == BookStatus.Reading)
                throw new BookDomainException("Already reading this book");

            var @event = new BookStartedDomainEvent(this.Id, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void FinishedReading()
        {
            if (Status == BookStatus.Finish)
                throw new BookDomainException("Already finished this book");
            
            if (Status == BookStatus.DNF)
                throw new BookDomainException("This book was not finished");
            
            var @event = new BookFinishedDomainEvent(this.Id, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void DecidedNotFinish()
        {
            if (Status == BookStatus.Finish)
                throw new BookDomainException("Already finished this book");
            var @event = new DecidedNotToFinishDomainEvent(this.Id, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        public void MarkBookAsWanted()
        {
            var @event = new MarkedBookAsWantedDomainEvent(this.Id, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }
        
        public void ReadToPage(int pageNumber)
        {
            if (pageNumber <= 0 || pageNumber > TotalPages)
            {
                throw new BookDomainException("Please enter valid page number");
            }
            
            var @event = new ReadToPageDomainEvent(this.Id, this.CurrentPageNumber, pageNumber, this.Version);
            this.AddEvent(@event);
            this.Apply(@event);
        }

        #region Apply Events
        
        private void Apply(DecidedNotToFinishDomainEvent @event)
        {
            Version++;
            Status = BookStatus.DNF;
        }

        private void Apply(BookFinishedDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Finish;
        }
        
        private void Apply(BookStartedDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Reading;
        }

        private void Apply(MarkedBookAsWantedDomainEvent @event)
        {
            Version++;
            Status = BookStatus.Want;
        }

        private void Apply(ReadToPageDomainEvent @event)
        {
            Version++;
            CurrentPageNumber = @event.NewPageNumber;
        }
        #endregion
    }
}