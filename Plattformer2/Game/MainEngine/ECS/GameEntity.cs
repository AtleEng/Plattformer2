using System.Collections.Generic;
using System.Numerics;
using Engine;
using Physics;

namespace Engine
{
    //The class that handle entitys
    public class GameEntity
    {
        //name of gameEntity
        public string name { get; set; } = "GameEntity";

        //If isActive the gameEntity is being used in the systems
        public bool isActive { get; set; } = true;
        //See Transform.cs
        public Transform transform { get; set; } = new(null, new(), Vector2.Zero, Vector2.One);
        //List of the entitys diffrent components
        public List<Component> components { get; set; } = new();

        public void OnTrigger(Collider other)//If entity has a trigger collider and it is triggered
        {
            foreach (Component component in components)
            {
                component.OnTrigger(other);
            }
        }
        public void OnCollision(Collider other)//If entity has a collider and it is colliding
        {
            foreach (Component component in components)
            {
                component.OnCollision(other);
            }
        }
        public virtual void OnInnit() { } //When spawned
        public string PrintStats()//Debug tree (Press F3)
        {
            return $"isActive({isActive}) transform({transform.position},{transform.size})";
        }
        public bool HasComponent<T>() where T : Component //Check if entity has a component of specified type
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
        }
        public T? GetComponent<T>() where T : Component //Get a component of specified type if the entity has it
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    return c as T;
                }
            }
            return null;
        }
        public T? GetComponentInterface<T>() where T : class
        {
            foreach (Component c in components)
            {
                if (c is T)
                {
                    return c as T;
                }
            }
            return null;
        }
        public void AddComponent<T>(Component component) where T : Component
        {
            component.gameEntity = this;
            components.Add(component);
        }
        public void RemoveComponent<T>()
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    components.Remove(c);
                    return;
                }
            }
        }
    }
}