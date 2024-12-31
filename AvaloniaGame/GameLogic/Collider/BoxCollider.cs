using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;

namespace AvaloniaGame.GameLogic.Collider
{
    public class BoxCollider : ICollider
    {
        public List<Vector3> collision { get; private set; }

        public List<Vector3> globalCollision { get; private set; }

        public BoxCollider(List<Vector3> vertices)
        {
            collision = vertices.ToList();
            globalCollision = vertices.ToList();
        }

        public void updateGlobalCollision(Vector3 position, Vector3 rotation)
        {
            this.globalCollision = new List<Vector3>();

            Matrix4 trans = Matrix4.Identity;
            trans = trans * Matrix4.CreateRotationX(rotation.X);
            trans = trans * Matrix4.CreateRotationY(rotation.Y);
            trans = trans * Matrix4.CreateRotationZ(rotation.Z);
            trans = trans * Matrix4.CreateTranslation(position);
            for (int i = 0; i < this.collision.Count; i++)
            {
                Vector3 curVertex = this.collision[i];
                Vector4 t1 = new Vector4(curVertex, 1);
                Vector4 t2 = Vector4.TransformRow(t1, trans);
                Vector3 newVertex = new Vector3(t2);
                this.globalCollision.Add(newVertex);
            }
        }
        private bool CollidesWith(BoxCollider other)
        {
            Vector3 minA = new Vector3
                (
                    this.globalCollision.Min(p => p.X),
                    this.globalCollision.Min(p => p.Y),
                    this.globalCollision.Min(p => p.Z)
                );
            Vector3 maxA = new Vector3
                (
                    this.globalCollision.Max(p => p.X),
                    this.globalCollision.Max(p => p.Y),
                    this.globalCollision.Max(p => p.Z)
                );

            Vector3 minB = new Vector3
                (
                    other.globalCollision.Min(p => p.X),
                    other.globalCollision.Min(p => p.Y),
                    other.globalCollision.Min(p => p.Z)
                );
            Vector3 maxB = new Vector3
                (
                    other.globalCollision.Max(p => p.X),
                    other.globalCollision.Max(p => p.Y),
                    other.globalCollision.Max(p => p.Z)
                );

            return minA.X < maxB.X && maxA.X > minB.X &&
            minA.Y < maxB.Y && maxA.Y > minB.Y &&
            minA.Z < maxB.Z && maxA.Z > minB.Z;
        }
        /*public Vector3 getCollisionSide(BoxCollider other)
        {
            if (!this.CollidesWith(other))
                return new Vector3(0, 0, 0);

            Vector3 minA = new Vector3
                (
                    this.globalCollision.Min(p => p.X),
                    this.globalCollision.Min(p => p.Y),
                    this.globalCollision.Min(p => p.Z)
                );
            Vector3 maxA = new Vector3
                (
                    this.globalCollision.Max(p => p.X),
                    this.globalCollision.Max(p => p.Y),
                    this.globalCollision.Max(p => p.Z)
                );

            Vector3 minB = new Vector3
                (
                    other.globalCollision.Min(p => p.X),
                    other.globalCollision.Min(p => p.Y),
                    other.globalCollision.Min(p => p.Z)
                );
            Vector3 maxB = new Vector3
                (
                    other.globalCollision.Max(p => p.X),
                    other.globalCollision.Max(p => p.Y),
                    other.globalCollision.Max(p => p.Z)
                );

            Vector3 overlap = new Vector3(0, 0, 0);
            overlap.X = Math.Min(maxA.X, maxB.X) - Math.Max(minA.X, minB.X);
            overlap.Y = Math.Min(maxA.Y, maxB.Y) - Math.Max(minA.Y, minB.Y);
            overlap.Z = Math.Min(maxA.Z, maxB.Z) - Math.Max(minA.Z, minB.Z);

            if (Math.Abs(overlap.X) < Math.Abs(overlap.Y) && Math.Abs(overlap.X) < Math.Abs(overlap.Z))
                return new Vector3(0, 1, 1);
            else if(Math.Abs(overlap.Y) < Math.Abs(overlap.X) && Math.Abs(overlap.Y) < Math.Abs(overlap.Z))
                return new Vector3(1, 0, 1);
            else return new Vector3(1, 1, 0);

        }
*/
        public bool CollidesWith(ICollider other)
        {
            if(other is BoxCollider)
            {
                return CollidesWith((BoxCollider)other);
            }
            return false;
        }




        public Vector3 getDiff(BoxCollider other)
        {
            if (!this.CollidesWith(other))
                return new Vector3(0, 0, 0);

            Vector3 minA = new Vector3
                (
                    this.globalCollision.Min(p => p.X),
                    this.globalCollision.Min(p => p.Y),
                    this.globalCollision.Min(p => p.Z)
                );
            Vector3 maxA = new Vector3
                (
                    this.globalCollision.Max(p => p.X),
                    this.globalCollision.Max(p => p.Y),
                    this.globalCollision.Max(p => p.Z)
                );

            Vector3 minB = new Vector3
                (
                    other.globalCollision.Min(p => p.X),
                    other.globalCollision.Min(p => p.Y),
                    other.globalCollision.Min(p => p.Z)
                );
            Vector3 maxB = new Vector3
                (
                    other.globalCollision.Max(p => p.X),
                    other.globalCollision.Max(p => p.Y),
                    other.globalCollision.Max(p => p.Z)
                );

            Vector3 res = new Vector3(0, 0, 0);
            res.X = Math.Abs(maxA.X - minB.X) < Math.Abs(maxB.X - minA.X) ? -(maxA.X - minB.X) : maxB.X - minA.X;
            res.Y = Math.Abs(maxA.Y - minB.Y) < Math.Abs(maxB.Y - minA.Y) ? -(maxA.Y - minB.Y) : maxB.Y - minA.Y;
            res.Z = Math.Abs(maxA.Z - minB.Z) < Math.Abs(maxB.Z - minA.Z) ? -(maxA.Z - minB.Z) : maxB.Z - minA.Z;

            if (Math.Abs(res.X) < Math.Abs(res.Y) && Math.Abs(res.X) < Math.Abs(res.Z))
                { return new Vector3(res.X, 0, 0); }
            else if (Math.Abs(res.Y) < Math.Abs(res.X) && Math.Abs(res.Y) < Math.Abs(res.Z) )
                { return new Vector3(0, res.Y, 0); }
            else
                { return new Vector3(0, 0, res.Z); }
        }
    }
}
