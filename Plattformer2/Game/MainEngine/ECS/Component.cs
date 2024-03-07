using System.Collections.Generic;
using System.Numerics;
using Engine;
using Physics;

namespace Engine
{
    [Serializable]
    public abstract class Component
    {
        public Component() { }

        public GameEntity gameEntity = new();

        public virtual void Start() { }
        public virtual void Update(float delta) { }
        public virtual void OnDestroy() { }
        public virtual void OnTrigger(Collider other) { }

        public virtual string PrintStats() { return ""; }
    }
}