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
            List<Tuple<Collider, float>> collidersAndArea = new();

            Rectangle aabb = GetRectangleFromCollider(collider);

            foreach (GameEntity otherGameEntity in Core.activeGameEntities)
            {
                if (otherGameEntity != collider.gameEntity)
                {
                    Collider? otherCollider = otherGameEntity.GetComponent<Collider>();
                    if (otherCollider != null)
                    {
                        if (PhysicsSettings.collisionMatrix[collider.layer, otherCollider.layer])
                        {
                            Rectangle otherAabb = GetRectangleFromCollider(otherCollider);

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
                                    collidersAndArea.Add(new(otherCollider, area));
                                }
                            }
                        }
                    }
                }
            }
            return SortOtherAabbsByArea(collidersAndArea); //Works
        }

        void SolveCollision(List<Collider> other, PhysicsBody physicsBody, Collider collider) //ToDo 
        {
            System.Console.WriteLine($"Name: {collider.gameEntity.name}");
            for (int i = 0; i < other.Count; i++)
            {
                Rectangle aabb = GetRectangleFromCollider(collider);
                Rectangle otherAabb = GetRectangleFromCollider(other[i]);

                Rectangle collisonRec = Raylib.GetCollisionRec(aabb, otherAabb);

                System.Console.WriteLine($"Overlap #{i}: {collisonRec.Width}|{collisonRec.Height}");

                //solve collision
                if (collisonRec.Width < collisonRec.Height && collisonRec.Width * collisonRec.Height != 0)
                {
                    if (aabb.X < otherAabb.X)
                    {
                        collider.gameEntity.transform.position.X = otherAabb.X - aabb.Width / 2;
                        // Adjust position before inverting velocity
                        physicsBody.velocity.X *= -physicsBody.elasticity;
                    }
                    else
                    {
                        collider.gameEntity.transform.position.X = otherAabb.X + otherAabb.Width + aabb.Width / 2;
                        // Adjust position before inverting velocity  
                        physicsBody.velocity.X *= -physicsBody.elasticity;
                    }
                }
                else if (collisonRec.Width * collisonRec.Height != 0)
                {
                    if (aabb.Y < otherAabb.Y)
                    {
                        collider.gameEntity.transform.position.Y = otherAabb.Y - aabb.Height / 2;
                        // Adjust position before inverting velocity  
                        physicsBody.velocity.Y *= -physicsBody.elasticity;
                    }
                    else
                    {
                        collider.gameEntity.transform.position.Y = otherAabb.Y + otherAabb.Height + aabb.Height / 2;
                        // Adjust position before inverting velocity  
                        physicsBody.velocity.Y *= -physicsBody.elasticity;
                    }
                }
            
            }
            Core.UpdateChildren(collider.gameEntity.transform.parent);
        }

        Rectangle GetRectangleFromCollider(Collider collider)
        {
            return new Rectangle
            (
                collider.gameEntity.transform.worldPosition.X + collider.offset.X - collider.gameEntity.transform.worldSize.X * collider.scale.X / 2,
                collider.gameEntity.transform.worldPosition.Y + collider.offset.Y - collider.gameEntity.transform.worldSize.Y * collider.scale.Y / 2,
                collider.gameEntity.transform.worldSize.X * collider.scale.X,
                collider.gameEntity.transform.worldSize.Y * collider.scale.Y
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