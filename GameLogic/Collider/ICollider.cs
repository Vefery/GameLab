using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.GameLogic.Collider
{
    public interface ICollider
    {
        public List<Vector3> collision { get; }
        public bool CollidesWith(ICollider other);
    }
}
