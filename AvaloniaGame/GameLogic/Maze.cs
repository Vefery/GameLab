using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

namespace AvaloniaGame.GameLogic
{
    public enum MazeDirection
    {
        left, right, up, down
    }
    public class Maze : GameObject
    {
        public Vector3 target;
        public int depth = 6;
        public int seed = -1;
        public bool goalSpawned = false;
        public Vector3 startPose;
        public List<Vector3> tilePositions = new();
        public List<Room> rooms = new();
        private int initDepth;
        public List<Action> roomsCreateActions = new();
        public ExitDoor exitDoor;
        public Random rand;

        public Maze(GL gl) : base(gl)
        {
            startPose = Vector3.Zero;
            tilePositions.Add(startPose);
            initDepth = depth;

            if(MainLogic.isMultiplayer)
            {
                if (MainLogic.networkManager.isServer)
                {
                    rand = new();
                    int curSeed = rand.Next();
                    MainLogic.networkManager.SendMessage("Seed: " + curSeed.ToString());
                    rand = new(curSeed);
                }
                else
                {
                    int seed;
                    if (!int.TryParse(MainLogic.seedString, out seed))
                        Console.WriteLine("Неверный формат сида.");
                    rand = new(seed);

                }
            }
            else
            {
                rand = new();
            }

            rooms.Add(MainLogic.Register(new Room(gl), position));

            if (MainLogic.difficulty == 0)
                depth = 6;
            else if (MainLogic.difficulty == 1)
                depth = 20;
            else if (MainLogic.difficulty == 2)
                depth = 50;
            GenerateMaze(gl);
        }

        public void GenerateMaze(GL gl)
        {
            rooms[0].Generate(this);
            for (int i = 0; i < roomsCreateActions.Count; i++)
            {
                roomsCreateActions[i]();
            }
            if (!goalSpawned)
            {
                goalSpawned = true;
                SpawnExit(gl, rooms.Last());
            }
            roomsCreateActions.Clear();
        }
        public void SpawnExit(GL gl, Room room)
        {
            if (room.left == null && CheckPosition(room.position + new Vector3(-room.halfWidth, 0, 0)))
                exitDoor = MainLogic.Register(new ExitDoor(gl), room.position);
            else if (room.right == null && CheckPosition(room.position + new Vector3(room.halfWidth, 0, 0)))
                exitDoor = MainLogic.Register(new ExitDoor(gl), room.position, new Vector3(0f, 180f, 0f));
            else if (room.up == null && CheckPosition(room.position + new Vector3(0, 0, room.halfWidth)))
                exitDoor = MainLogic.Register(new ExitDoor(gl), room.position, new Vector3(0f, 90f, 0f));
            else
                exitDoor = MainLogic.Register(new ExitDoor(gl), room.position, new Vector3(0f, -90f, 0f));
        }
        public void ResetMaze(GL gl)
        {
            depth = initDepth;
            goalSpawned = false;
            tilePositions.Clear();
            /*foreach (Room room in rooms)
                Destroy(room.gameObject);*/
            rooms.Clear();
            rooms.Add(MainLogic.Register(new Room(gl), position));
            tilePositions.Add(rooms[0].position);
            GenerateMaze(gl);
        }
        public bool CheckPosition(Vector3 pos)
        {
            foreach (Vector3 p in tilePositions)
            {
                if (Vector3.Distance(p, pos) < 5)
                    return false;
            }
            return true;
        }

        public override void Update(float deltaTime)
        {
            ;
        }

        public override void Start(GL gl)
        {
            ;
        }
    }
}
