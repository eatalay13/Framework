﻿using Entities.Models;

namespace Core.Events
{
    /// <summary>
    ///     A container for passing entities that have been deleted. This is not used for entities that are deleted logically
    ///     via a bit column.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeletedEvent<T> where T : BaseModel
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityDeletedEvent(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        ///     Entity
        /// </summary>
        public T Entity { get; }
    }
}