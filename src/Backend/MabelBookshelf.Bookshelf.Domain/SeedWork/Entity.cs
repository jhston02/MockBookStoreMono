﻿namespace MabelBookshelf.Bookshelf.Domain.SeedWork
{
    public abstract class Entity<T>
    {
        public T Id { get; protected set; }

        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Entity<T> other)
        {
            if (Id.Equals(other.Id))
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity<T>)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual void Apply(DomainEvent @event)
        {
        }
        
        
    }
}