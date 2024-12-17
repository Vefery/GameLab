﻿using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public enum MazeDirection
    {
        left, right, up, down
    }
    public class Maze : GameObject
    {
        public Vector3 target;
        public int depth = 6;
        public bool goalSpawned = false;
        public Vector3 startPose;
        public List<Vector3> tilePositions = new();
        public List<Room> rooms = new();
        private int initDepth;
        public List<Action> roomsCreateActions = new();
        public ExitDoor exitDoor;

        public Maze()
        {
            startPose = Vector3.Zero;
            tilePositions.Add(startPose);
            initDepth = depth;
            rooms.Add(MainLogic.Instantiate<Room>(position));

            if (MainLogic.difficulty == 0)
                depth = 6;
            else if (MainLogic.difficulty == 1)
                depth = 20;
            else if (MainLogic.difficulty == 2)
                depth = 50;
            GenerateMaze();
        }

        public void GenerateMaze()
        {
            rooms[0].Generate(this);
            for (int i = 0; i < roomsCreateActions.Count; i++)
            {
                roomsCreateActions[i]();
            }
            if (!goalSpawned)
            {
                goalSpawned = true;
                SpawnExit(rooms.Last());
            }
            roomsCreateActions.Clear();
        }
        public void SpawnExit(Room room)
        {
            if (room.left == null)
                exitDoor = MainLogic.Instantiate<ExitDoor>(room.position);
            else if (room.right == null)
                exitDoor = MainLogic.Instantiate<ExitDoor>(room.position, new Vector3(0f, 180f, 0f));
            else if (room.up == null)
                exitDoor = MainLogic.Instantiate<ExitDoor>(room.position, new Vector3(0f, 90f, 0f));
            else
                exitDoor = MainLogic.Instantiate<ExitDoor>(room.position, new Vector3(0f, -90f, 0f));
        }
        public void ResetMaze()
        {
            depth = initDepth;
            goalSpawned = false;
            tilePositions.Clear();
            /*foreach (Room room in rooms)
                Destroy(room.gameObject);*/
            rooms.Clear();
            rooms.Add(MainLogic.Instantiate<Room>(position));
            tilePositions.Add(rooms[0].position);
            GenerateMaze();
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
    }
}
