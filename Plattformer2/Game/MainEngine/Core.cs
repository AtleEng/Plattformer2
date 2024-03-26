using System.Collections.Generic;
using System.Numerics;
using System.IO;
using Raylib_cs;

using Engine;
using Animation;
using Physics;
using Graphics;


namespace CoreEngine
{
    public static class Core
    {
        public static GameEntity currentScene = new Scene();
        public static bool shouldClose;

        static public List<GameEntity> gameEntities = new();
        static public List<GameEntity> activeGameEntities = new();

        static public List<GameSystem> systems = new();

        static public List<GameEntity> entitiesToAdd = new();
        static public List<GameEntity> entitiesToRemove = new();

        //deltaTime variabler
        static float oldTime = 0;
        static float newTime = 0;

        public static double maxPhysicsTimeStep = 0.1; // 0.1 seconds
        public static double physicsTimeStep = 0.1;
        public static void Start()
        {
            AddSystem<ScriptSystem>();
            AddSystem<PhysicsSystem>();
            AddSystem<CollisionSystem>();
            AddSystem<AnimationSystem>();
            AddSystem<RenderSystem>();

            // Innit all the systems in the right order
            systems[systems.Count - 1].Start();

            for (int i = 0; i < systems.Count - 1; i++)
            {
                systems[i].Start();
            }
            //Console.Clear();
            currentScene.OnInnit();

            while (shouldClose == false)
            {
                oldTime = newTime;
                newTime = (float)Raylib.GetTime();
                float deltaTime = newTime - oldTime;

                Update(deltaTime);
            }
        }
        static void Update(float delta)
        {
            activeGameEntities.Clear();

            GetAllActiveEntities(currentScene);

            // Uppdate all the systems in the right order
            // TODO nu hoppar den över physics om delta är för hög inte jättebra lösning men fungerar
            for (int i = 0; i < systems.Count; i++)
            {
                if (!(delta > 0.02f && (i == 1 || i == 2)))
                {
                    systems[i].Update(delta);
                }
                else
                {
                    //System.Console.WriteLine(delta);
                }
            }

            UpdateChildren(currentScene.transform);
            // Add and remove games entities
            foreach (var entity in entitiesToAdd)
            {
                gameEntities.Add(entity);
            }
            foreach (var entity in entitiesToRemove)
            {
                if (entity.transform.parent != null)
                {
                    entity.transform.parent.children.Remove(entity.transform);
                }
                gameEntities.Remove(entity);
            }
            //clear the lists
            entitiesToAdd.Clear();
            entitiesToRemove.Clear();
            if (Raylib.IsKeyPressed(KeyboardKey.F3))
            {
                //Console.Clear();
                PrintEntityTree(currentScene, "", "");
            }
        }

        static public void UpdateChildren(Engine.Transform parent)
        {
            foreach (Engine.Transform child in parent.children)
            {
                child.worldPosition = child.position + parent.worldPosition;
                child.worldSize = child.size * parent.worldSize;

                UpdateChildren(child);
            }
        }

        public static void AddSystem<T>() where T : GameSystem
        {
            GameSystem system = (GameSystem)Activator.CreateInstance(typeof(T));
            systems.Add(system);
        }
        public static void RemoveSystem<T>() where T : GameSystem
        {
            foreach (GameSystem gS in systems)
            {
                if (gS.GetType() == typeof(T))
                {
                    systems.Remove(gS);
                }
            }
        }

        static void PrintEntityTree(GameEntity entity, string layer = "", string space = "")
        {
            Console.WriteLine($"{space}{layer}{entity.name} [{entity.PrintStats()}]");

            // Components
            if (entity.components.Count > 0)
            {
                foreach (Component component in entity.components)
                {
                    Console.WriteLine($"   {space}{component.GetType().Name} [{component.PrintStats()}]");
                }
            }
            // Entities
            if (entity.transform.children.Count > 0)
            {
                foreach (var child in entity.transform.children)
                {
                    PrintEntityTree(child.gameEntity, $"{layer}>", $"{space} ");
                }

            }
        }
        static void GetAllActiveEntities(GameEntity entity)
        {
            if (entity.transform.children.Count > 0)
            {
                foreach (Engine.Transform child in entity.transform.children)
                {
                    if (child.gameEntity.isActive)
                    {
                        activeGameEntities.Add(child.gameEntity);
                        GetAllActiveEntities(child.gameEntity);
                    }
                }

            }
        }
    }
}

namespace Engine
{
    public class Scene : GameEntity
    {
        public override void OnInnit()
        {
            name = "Scene";

            EntityManager.SpawnEntity(new GameManager(), Vector2.Zero);
        }
    }
}