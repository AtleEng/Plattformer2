using System.Collections.Generic;
using System.Numerics;
using Engine;
using Physics;

namespace Engine
{
    public class GameEntity
    {
        public GameEntity() { }

        public string name { get; set; } = "GameEntity";

        public bool isActive { get; set; } = true;
        public Transform transform { get; set; } = new(null, new(), Vector2.Zero, Vector2.One);
        public List<Component> components { get; set; } = new();

        public void OnTrigger(Collider other)
        {
            foreach (Component component in components)
            {
                component.OnTrigger(other);
            }
        }
        public void OnCollision(Collider other)
        {
            foreach (Component component in components)
            {
                component.OnCollision(other);
            }
        }
        public virtual void OnInnit()
        {

        }
        public string PrintStats()
        {
            return $"isActive: {isActive}- -transform:{transform.position},{transform.size}-";
        }
        public bool HasComponent<T>() where T : Component
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
        public T? GetComponent<T>() where T : Component
        {
            foreach (Component c in components)
            {
                if (c.GetType() == typeof(T))
                {
                    return (T)c;
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