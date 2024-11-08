using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaGame.GameLogic
{
    public enum MazeDirection
    {
        left, right, up, down
    }
    public class Maze : GameObject
    {
        public Vector3 target;
        public int depth = 10;
        public bool goalSpawned = false;
        public GameObject roomPrefab, wallPrefab, targetPrefab;
        public Vector3 startPose;
        public List<Vector3> tilePositions = new();
        public List<Room> rooms = new();
        private int initDepth;
        public List<Action> roomsCreateActions = new();
        public int roomsCount { get { return rooms.Count; } }

        public override void Awake()
        {
            startPose = Vector3.Zero;
            tilePositions.Add(startPose);
            initDepth = depth;
            rooms.Add(Instantiate(roomPrefab, this.position).GetComponent<Room>());
        }

        public override void Start()
        {
            GenerateMaze();
        }

        public override void Update()
        {
            throw new NotImplementedException();
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
                target = Instantiate(targetPrefab, rooms.Last().position, Quaternion.Identity).transform;
            }
            roomsCreateActions.Clear();
        }
        public void ResetMaze()
        {
            depth = initDepth;
            goalSpawned = false;
            tilePositions.Clear();
            foreach (Room room in rooms)
                Destroy(room.gameObject);
            rooms.Clear();
            rooms.Add(Instantiate(roomPrefab).GetComponent<Room>());
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
