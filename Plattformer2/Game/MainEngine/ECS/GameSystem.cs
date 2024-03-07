using System.Collections.Generic;
using System.Numerics;
using Engine;
using Physics;

namespace Engine
{
    public abstract class GameSystem
    {
        public List<Component> validComponents = new();
        public virtual void Start() { }
        public virtual void Update(float delta) { }
    }
    public interface IScript { }
}