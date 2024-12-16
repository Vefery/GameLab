﻿using MazeGame.GameLogic.Collider;
using MazeGame.Utils;
using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public class ExitDoor : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        private GameObject player;
        public ExitDoor()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models/ExitDoor.model");
            player = MainLogic.gameObjects.OfType<Player>().Single();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Vector3.Distance(position, player.position) < 1)
                MainLogic.finishFlag = true;
        }
    }
}
