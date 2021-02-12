using Entities.Models;

namespace Core.Events
{
    /// <summary>
    ///     A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInsertedEvent<T> where T : BaseModel
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        ///     Entity
        /// </summary>
        public T Entity { get; }
    }
}