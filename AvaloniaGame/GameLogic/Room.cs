using Avalonia.Media;
using Avalonia.OpenGL;
using AvaloniaGame.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaGame.GameLogic
{
    public class Room : GameObject
    {
        public int halfWidth = 10;
        public float neighbourChance = 0.25f;
        public Room? left, right, up, down;
        private bool leftAllowed = true, rightAllowed = true, upAllowed = true, downAllowed = true;
        private Random rand = new();

        public void Generate(Maze maze)
        {
            GenerationLoop(maze);
        }
        public void GenerateRooms(Maze maze, Room parentRoom, MazeDirection parentDirection)
        {
            switch (parentDirection)
            {
                case MazeDirection.left:
                    left = parentRoom;
                    leftAllowed = false;
                    break;
                case MazeDirection.right:
                    right = parentRoom;
                    rightAllowed = false;
                    break;
                case MazeDirection.up:
                    up = parentRoom;
                    upAllowed = false;
                    break;
                case MazeDirection.down:
                    down = parentRoom;
                    downAllowed = false;
                    break;
                default:
                    break;
            }
            maze.rooms.Add(this);
            maze.depth--;
            GenerationLoop(maze);
            if (maze.depth <= 0 && !maze.goalSpawned)
            {
                maze.goalSpawned = true;
                //maze.target = Instantiate(maze.targetPrefab, this.position, Quaternion.Identity).transform;
            }
        }

        public void Render(GlInterface gl)
        {
            throw new NotImplementedException();
        }

        private void GenerationLoop(Maze maze)
        {
            int roomsGenerated = 0;
            while (roomsGenerated == 0 && maze.depth > 0 && (rightAllowed || leftAllowed || upAllowed || downAllowed))
            {
                leftAllowed = maze.CheckPosition(this.position + new Vector3(-halfWidth, 0, 0)) && left == null;
                if (rand.NextDouble() <= neighbourChance && leftAllowed)
                {
                    //left = Instantiate(maze.roomPrefab, this.position + new Vector3(-halfWidth, 0, 0), Quaternion.Identity).GetComponent<Room>();
                    maze.tilePositions.Add(this.position + new Vector3(-halfWidth, 0, 0));
                    maze.roomsCreateActions.Add(() => left.GenerateRooms(maze, this, MazeDirection.right));
                    roomsGenerated++;
                }
                rightAllowed = maze.CheckPosition(this.position + new Vector3(halfWidth, 0, 0)) && right == null;
                if (rand.NextDouble() <= neighbourChance && rightAllowed)
                {
                    //right = Instantiate(maze.roomPrefab, this.position + new Vector3(halfWidth, 0, 0), Quaternion.Identity).GetComponent<Room>();
                    maze.tilePositions.Add(this.position + new Vector3(halfWidth, 0, 0));
                    maze.roomsCreateActions.Add(() => right.GenerateRooms(maze, this, MazeDirection.left));
                    roomsGenerated++;
                }
                upAllowed = maze.CheckPosition(this.position + new Vector3(0, 0, halfWidth)) && up == null;
                if (rand.NextDouble() <= neighbourChance && upAllowed)
                {
                    //up = Instantiate(maze.roomPrefab, this.position + new Vector3(0, 0, halfWidth), Quaternion.Identity).GetComponent<Room>();
                    maze.tilePositions.Add(this.position + new Vector3(0, 0, halfWidth));
                    maze.roomsCreateActions.Add(() => up.GenerateRooms(maze, this, MazeDirection.down));
                    roomsGenerated++;
                }
                downAllowed = maze.CheckPosition(this.position + new Vector3(0, 0, -halfWidth)) && down == null;
                if (rand.NextDouble() <= neighbourChance && downAllowed)
                {
                    //down = Instantiate(maze.roomPrefab, this.position + new Vector3(0, 0, -halfWidth), Quaternion.Identity).GetComponent<Room>();
                    maze.tilePositions.Add(this.position + new Vector3(0, 0, -halfWidth));
                    maze.roomsCreateActions.Add(() => down.GenerateRooms(maze, this, MazeDirection.up));
                    roomsGenerated++;
                }
            }

            /*if (left == null)
                Instantiate(maze.wallPrefab, this.position, Quaternion.Identity);
            if (right == null)
                Instantiate(maze.wallPrefab, this.position, Quaternion.CreateFromAxisAngle(Vector3.UnitY, 180));
            if (up == null)
                Instantiate(maze.wallPrefab, this.position, Quaternion.CreateFromAxisAngle(Vector3.UnitY, 90));
            if (down == null)
                Instantiate(maze.wallPrefab, this.position, Quaternion.CreateFromAxisAngle(Vector3.UnitY, -90));*/
        }
    }
}
