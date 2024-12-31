using System;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

namespace AvaloniaGame.GameLogic
{
    public class Room(GL gl) : GameObject(gl)
    {
        public float halfWidth = 5f;
        public float neighbourChance = 0.25f;
        public Room? left, right, up, down;
        private bool leftAllowed = true, rightAllowed = true, upAllowed = true, downAllowed = true;
        private Random rand = new();

        public override void Start(GL gl)
        {
            MainLogic.Register(new FloorPrefab(gl), position);
            MainLogic.Register(new CeilingPrefab(gl), position);
        }

        public override void Update(float deltaTime)
        {
            ;
        }

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
                maze.SpawnExit(gl, this);
            }
        }

        private void GenerationLoop(Maze maze)
        {
            int roomsGenerated = 0;
            while (roomsGenerated == 0 && maze.depth > 0 && (rightAllowed || leftAllowed || upAllowed || downAllowed))
            {
                leftAllowed = maze.CheckPosition(this.position + new Vector3(-halfWidth, 0, 0)) && left == null;
                if (rand.NextDouble() <= neighbourChance && leftAllowed)
                {
                    left = MainLogic.Register(new Room(gl), position + new Vector3(-halfWidth, 0, 0));
                    maze.tilePositions.Add(this.position + new Vector3(-halfWidth, 0, 0));
                    maze.roomsCreateActions.Add(() => left.GenerateRooms(maze, this, MazeDirection.right));
                    roomsGenerated++;
                }
                rightAllowed = maze.CheckPosition(this.position + new Vector3(halfWidth, 0, 0)) && right == null;
                if (rand.NextDouble() <= neighbourChance && rightAllowed)
                {
                    right = MainLogic.Register(new Room(gl), position + new Vector3(halfWidth, 0, 0));
                    maze.tilePositions.Add(this.position + new Vector3(halfWidth, 0, 0));
                    maze.roomsCreateActions.Add(() => right.GenerateRooms(maze, this, MazeDirection.left));
                    roomsGenerated++;
                }
                upAllowed = maze.CheckPosition(this.position + new Vector3(0, 0, halfWidth)) && up == null;
                if (rand.NextDouble() <= neighbourChance && upAllowed)
                {
                    up = MainLogic.Register(new Room(gl), position + new Vector3(0, 0, halfWidth));
                    maze.tilePositions.Add(this.position + new Vector3(0, 0, halfWidth));
                    maze.roomsCreateActions.Add(() => up.GenerateRooms(maze, this, MazeDirection.down));
                    roomsGenerated++;
                }
                downAllowed = maze.CheckPosition(this.position + new Vector3(0, 0, -halfWidth)) && down == null;
                if (rand.NextDouble() <= neighbourChance && downAllowed)
                {
                    down = MainLogic.Register(new Room(gl), position + new Vector3(0, 0, -halfWidth));
                    maze.tilePositions.Add(this.position + new Vector3(0, 0, -halfWidth));
                    maze.roomsCreateActions.Add(() => down.GenerateRooms(maze, this, MazeDirection.up));
                    roomsGenerated++;
                }
            }

            if (left == null)
                MainLogic.Register(new WallPrefab(gl), position);
            if (right == null)
                MainLogic.Register(new WallPrefab(gl), position, new Vector3(0f, 180f, 0f));
            if (up == null)
                MainLogic.Register(new WallPrefab(gl), position, new Vector3(0f, 90f, 0f));
            if (down == null)
                MainLogic.Register(new WallPrefab(gl), position, new Vector3(0f, -90f, 0f));
        }
    }
}
