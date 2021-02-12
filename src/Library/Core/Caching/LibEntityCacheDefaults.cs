using Entities.Models;

namespace Core.Caching
{
    /// <summary>
    ///     Represents default values related to caching entities
    /// </summary>
    public static class LibEntityCacheDefaults<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        ///     Gets an entity type name used in cache keys
        /// </summary>
        public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

        /// <summary>
        ///     Gets a key for caching entity by identifier
        /// </summary>
        /// <remarks>
        ///     {0} : entity id
        /// </remarks>
        public static CacheKey ByIdCacheKey => new CacheKey($"Lib.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

        /// <summary>
        ///     Gets a key for caching entities by identifiers
        /// </summary>
        /// <remarks>
        ///     {0} : entity ids
        /// </remarks>
        public static CacheKey ByIdsCacheKey => new CacheKey($"Lib.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

        /// <summary>
        ///     Gets a key for caching all entities
        /// </summary>
        public static CacheKey AllCacheKey => new CacheKey($"Lib.{EntityTypeName}.all.", AllPrefix, Prefix);

        /// <summary>
        ///     Gets a key pattern to clear cache
        /// </summary>
        public static string Prefix => $"Lib.{EntityTypeName}.";

        /// <summary>
        ///     Gets a key pattern to clear cache
        /// </summary>
        public static string ByIdPrefix => $"Lib.{EntityTypeName}.byid.";

        /// <summary>
        ///     Gets a key pattern to clear cache
        /// </summary>
        public static string ByIdsPrefix => $"Lib.{EntityTypeName}.byids.";

        /// <summary>
        ///     Gets a key pattern to clear cache
        /// </summary>
        public static string AllPrefix => $"Lib.{EntityTypeName}.all.";
    }
}