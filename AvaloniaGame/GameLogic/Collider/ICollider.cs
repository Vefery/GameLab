using System.Collections.Generic;

using OpenTK.Mathematics;

namespace AvaloniaGame.GameLogic.Collider
{
    public interface ICollider
    {
        public List<Vector3> collision { get; }
        public bool CollidesWith(ICollider other);
    }
}
