using System.Collections.Generic;
using System.Numerics;
using CoreEngine;
using Engine;

namespace Engine
{
    //handles all entities for the components
    public static class EntityManager
    {
        public static void SpawnEntity(GameEntity entity) { SpawnEntity(entity, Vector2.Zero); }
        public static void SpawnEntity(GameEntity entity, Vector2 position) { SpawnEntity(entity, position, Vector2.One); }
        public static void SpawnEntity(GameEntity entity, Vector2 position, Vector2 size) { SpawnEntity(entity, position, size, null); }
        public static void SpawnEntity(GameEntity entity, Vector2 position, Vector2 size, Transform parent)
        {
            entity.transform.position = position;
            entity.transform.size = size;

            entity.transform.gameEntity = entity;

            if (parent != null)
            {
                entity.transform.parent = parent;
            }
            else
            {
                entity.transform.parent = Core.currentScene.transform;
            }
            entity.transform.parent.children.Add(entity.transform);

            entity.OnInnit();
            foreach (Component component in entity.components)
            {
                component.Start();
            }
            Core.entitiesToAdd.Add(entity);
        }
        public static void DestroyEntity(GameEntity entity)
        {
            foreach (Component component in entity.components)
            {
                component.OnDestroy();
            }
            foreach (Transform child in entity.transform.children)
            {
                DestroyEntity(child.gameEntity);
            }
            Core.entitiesToRemove.Add(entity);
        }
    }
}