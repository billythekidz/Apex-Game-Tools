namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// This static class provides a way for entities to register themselves using their collider as a key.
    /// This enables an entity lookup in physics related cases, e.g. from OverlapSphere.
    /// The EntityManager uses static extension methods for ease of use and convenience.
    /// </summary>
    public static class EntityManager
    {
        private static readonly Dictionary<Collider, IEntity> _entities = new Dictionary<Collider, IEntity>(100);

        private static readonly Dictionary<Type, EntityType> _typesLookup = new Dictionary<Type, EntityType>()
        {
            { typeof(IUnit), EntityType.Unit },
            { typeof(IResource), EntityType.Resource },
            { typeof(IStructure), EntityType.Structure }
        };

        /// <summary>
        /// Links the entity to the specified collider.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="collider">The collider.</param>
        public static void Link(this IEntity entity, Collider collider)
        {
            _entities[collider] = entity;
        }

        /// <summary>
        /// Unlinks the the entity from the specified collider.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="collider">The collider.</param>
        /// <returns></returns>
        public static bool Unlink(this IEntity entity, Collider collider)
        {
            return _entities.Remove(collider);
        }

        /// <summary>
        /// Gets all entities of the specified type, cast to the desired type, as an enumerable collection (to avoid allocations).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllEntities<T>() where T : IEntity
        {
            var type = _typesLookup.GetValueOrDefault(typeof(T), EntityType.None);
            using (var enumerator = _entities.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var ent = enumerator.Current.Value;
                    if (ent.entityType == type)
                    {
                        yield return (T)ent;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the entity associated with the specified collider, or null if none is registered.
        /// </summary>
        /// <param name="collider">The collider.</param>
        /// <returns></returns>
        public static IEntity GetEntity(this Collider collider)
        {
            return _entities.GetValueOrDefault(collider);
        }

        /// <summary>
        /// Gets the entity associated with the specified collider, cast to the desired type.
        /// </summary>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <param name="collider">The collider.</param>
        /// <returns></returns>
        public static T GetEntity<T>(this Collider collider) where T : class, IEntity
        {
            return GetEntity(collider) as T;
        }

        /// <summary>
        /// Gets the entity referenced from the specified observation, cast to the desired type.
        /// </summary>
        /// <typeparam name="T">The desired type.</typeparam>
        /// <param name="observation">The observation.</param>
        /// <returns></returns>
        public static T GetEntity<T>(this Observation observation) where T : class, IEntity
        {
            return observation.entity as T;
        }
    }
}