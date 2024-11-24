using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public enum MazeDirection
    {
        left, right, up, down
    }
    public class Maze : GameObject
    {
        public Vector3 target;
        public int depth = 50;
        public bool goalSpawned = false;
        public Vector3 startPose;
        public List<Vector3> tilePositions = new();
        public List<Room> rooms = new();
        private int initDepth;
        public List<Action> roomsCreateActions = new();

        public Maze()
        {
            startPose = Vector3.Zero;
            tilePositions.Add(startPose);
            initDepth = depth;
            rooms.Add(MainLogic.Instantiate<Room>(position));

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
                //target = Instantiate(targetPrefab, rooms.Last().position, Quaternion.Identity).transform;
            }
            roomsCreateActions.Clear();
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
