using System.Numerics;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace Physics
{
    public class CollisionSystem : GameSystem
    {
        public override void Update(float delta)
        {
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                Collider? collider = gameEntity.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isColliding = false;
                }
            }
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                PhysicsBody? physicsBody = gameEntity.GetComponent<PhysicsBody>();
                Collider? collider = gameEntity.GetComponent<Collider>();

                if (collider != null && physicsBody != null)
                {
                    List<Collider> colliders = DetectCollision(collider, physicsBody); //works

                    if (!collider.isTrigger)
                    {
                        SolveCollision(colliders, physicsBody, collider);
                        Core.UpdateChildren(physicsBody.gameEntity.transform.parent);
                    }
                }
            }
        }
        List<Collider> DetectCollision(Collider collider, PhysicsBody physicsBody)
        {
            List<Tuple<Collider, float>> colliders = new();

            Rectangle aabb = GetCollisionRectangleFromCollider(collider.gameEntity, collider);

            foreach (GameEntity otherGameEntity in Core.activeGameEntities)
            {
                if (otherGameEntity != collider.gameEntity)
                {
                    Collider? otherCollider = otherGameEntity.GetComponent<Collider>();
                    if (otherCollider != null)
                    {
                        if (PhysicsSettings.collisionMatrix[collider.layer, otherCollider.layer])
                        {
                            Rectangle otherAabb = GetCollisionRectangleFromCollider(otherGameEntity, otherCollider);

                            Rectangle collisonRec = Raylib.GetCollisionRec(aabb, otherAabb);
                            float area = collisonRec.Width * collisonRec.Height;
                            if (area != 0)
                            {
                                collider.isColliding = true;
                                otherCollider.isColliding = true;
                                if (collider.isTrigger || otherCollider.isTrigger)
                                {
                                    collider.gameEntity.OnTrigger(otherCollider);
                                    otherCollider.gameEntity.OnTrigger(collider);
                                }
                                else if (physicsBody != null)
                                {
                                    colliders.Add(new(otherCollider, area));
                                }
                            }
                        }
                    }
                }
            }
            return SortOtherAabbsByArea(colliders); //Works
        }

        void SolveCollision(List<Collider> other, PhysicsBody physicsBody, Collider collider)
        {
            Rectangle aabb = GetCollisionRectangleFromCollider(collider.gameEntity, collider);

            List<Rectangle> otherAabbs = new();
            foreach (Collider c in other)
            {
                otherAabbs.Add(GetCollisionRectangleFromCollider(c.gameEntity, c));
            }

            for (int i = 0; i < otherAabbs.Count; i++)
            {
                float xOverlap = Math.Min(aabb.X + aabb.Width, otherAabbs[i].X + otherAabbs[i].Width) - Math.Max(aabb.X, otherAabbs[i].X);
                float yOverlap = Math.Min(aabb.Y + aabb.Height, otherAabbs[i].Y + otherAabbs[i].Height) - Math.Max(aabb.Y, otherAabbs[i].Y);

                if (xOverlap < yOverlap)
                {
                    int direction = Math.Sign(physicsBody.velocity.X);

                    if (aabb.X < otherAabbs[i].X)
                    {
                        collider.gameEntity.transform.position = new Vector2(otherAabbs[i].X - aabb.Width / 2, collider.gameEntity.transform.position.Y);
                    }
                    else
                    {
                        collider.gameEntity.transform.position = new Vector2(otherAabbs[i].X + otherAabbs[i].Width + aabb.Width / 2, collider.gameEntity.transform.position.Y);
                    }

                    // Adjust position before inverting velocity
                    physicsBody.velocity.X *= -direction * physicsBody.elasticity;
                }
                else
                {
                    int direction = Math.Sign(physicsBody.velocity.Y);

                    if (aabb.Y < otherAabbs[i].Y)
                    {
                        collider.gameEntity.transform.position = new Vector2(collider.gameEntity.transform.position.X, otherAabbs[i].Y - aabb.Height / 2);
                    }
                    else
                    {
                        collider.gameEntity.transform.position = new Vector2(collider.gameEntity.transform.position.X, otherAabbs[i].Y + otherAabbs[i].Height + aabb.Height / 2);
                    }

                    // Adjust position before inverting velocity
                    physicsBody.velocity.Y *= -direction * physicsBody.elasticity;
                }
                //update all collider to new pos
                for (int j = i; j < otherAabbs.Count; j++)
                {
                    float newXOverlap = Math.Min(otherAabbs[i].X + otherAabbs[i].Width, otherAabbs[j].X + otherAabbs[j].Width) - Math.Max(otherAabbs[i].X, otherAabbs[j].X);
                    float newYOverlap = Math.Min(otherAabbs[i].Y + otherAabbs[i].Height, otherAabbs[j].Y + otherAabbs[j].Height) - Math.Max(otherAabbs[i].Y, otherAabbs[j].Y);

                    if (newXOverlap < newYOverlap)
                    {
                        float overlapDirection = Math.Sign(otherAabbs[j].X - otherAabbs[i].X);

                        otherAabbs[j] = new Rectangle(otherAabbs[j].X + overlapDirection * newXOverlap, otherAabbs[j].Y, otherAabbs[j].Width, otherAabbs[j].Height);
                    }
                    else
                    {
                        float overlapDirection = Math.Sign(otherAabbs[j].Y - otherAabbs[i].Y);

                        otherAabbs[j] = new Rectangle(otherAabbs[j].X, otherAabbs[j].Y + overlapDirection * newYOverlap, otherAabbs[j].Width, otherAabbs[j].Height);
                    }
                }
            }
            Core.UpdateChildren(collider.gameEntity.transform.parent);
        }

        Rectangle GetCollisionRectangleFromCollider(GameEntity entity, Collider collider)
        {
            return new Rectangle
            (
                entity.transform.worldPosition.X + collider.offset.X - entity.transform.worldSize.X * collider.scale.X / 2,
                entity.transform.worldPosition.Y + collider.offset.Y - entity.transform.worldSize.Y * collider.scale.Y / 2,
                entity.transform.worldSize.X * collider.scale.X,
                entity.transform.worldSize.Y * collider.scale.Y
            );
        }

        List<Collider> SortOtherAabbsByArea(List<Tuple<Collider, float>> other) // works
        {
            // Sort the tuples by area in descending order
            var sortedTuples = other.OrderByDescending(t => t.Item2);

            // Extract the colliders from the sorted tuples
            List<Collider> sortedColliders = sortedTuples.Select(t => t.Item1).ToList();

            return sortedColliders;
        }
        public static class PhysicsSettings
        {
            public static bool[,] collisionMatrix = new bool[4, 4]
            {
            //true = collide / false = ignore collision
            //player ground check enemy
            { false, true, false, true}, //player
            { true, false, true, true}, //ground
            { false, true, false, false}, //check
            { false, true, false, false} //enemy
            };
        }
    }
}