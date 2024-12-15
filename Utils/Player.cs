using MazeGame.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using MazeGame.GameLogic.Collider;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.Mime.MediaTypeNames;

namespace MazeGame.Utils
{
    internal class Player : GameObject
    {
        private float movementSpeed;
        private float gravity;
        private float velocityY;
        private Vector3 cameraPosition;
        public Camera camera;

        bool flyMode;

        public BoxCollider collider { get; private set; }

        public Player(Vector3 _position, Vector3 _size, float _cameraHeight, float _speed, float _gravity)
        {
            position = _position;
            movementSpeed = _speed;
            gravity = _gravity;
            velocityY = 0;
            flyMode = false;

            cameraPosition = new Vector3(0, _cameraHeight, 0);
            camera = new Camera(cameraPosition, new Vector3(0.0f, 1.0f, 0.0f));

            collider = new BoxCollider(new List<Vector3>
            {
                new Vector3(-_size.X/2, 0, -_size.Z/2),
                new Vector3(-_size.X/2, 0, _size.Z/2),
                new Vector3(-_size.X/2, _size.Y, -_size.Z/2),
                new Vector3(-_size.X/2, _size.Y, _size.Z/2),
                new Vector3(_size.X/2, 0f, -_size.Z/2),
                new Vector3(_size.X/2, 0f, _size.Z/2),
                new Vector3(_size.X/2, _size.Y, -_size.Z/2),
                new Vector3(_size.X/2, _size.Y, _size.Z/2)
            });
        }

        public override void Update(float deltaTime)
        {
            var keyboard = MainLogic.keyboardState;
            var mouse = MainLogic.mouseState;

            camera.processMouseMovement(mouse.Delta.X, mouse.Delta.Y);

            if (keyboard.IsKeyPressed(Keys.F))
            {
                if(!flyMode)
                    velocityY = 0f;
                flyMode = !flyMode;
            }

            if (keyboard.IsKeyDown(Keys.Space))
                ProcessKeyboard(Keys.Space, deltaTime);
            if (keyboard.IsKeyDown(Keys.W))
                ProcessKeyboard(Keys.W, deltaTime);
            if (keyboard.IsKeyDown(Keys.S))
                ProcessKeyboard(Keys.S, deltaTime);
            if (keyboard.IsKeyDown(Keys.A))
                ProcessKeyboard(Keys.A, deltaTime);
            if (keyboard.IsKeyDown(Keys.D))
                ProcessKeyboard(Keys.D, deltaTime);

            if(!flyMode)
            {
                velocityY += gravity * deltaTime;
                position -= new Vector3(0, velocityY, 0);
            }
            HandleCollisions();
            camera.position = position + cameraPosition;
        }

        public void ProcessKeyboard(Keys key, float deltaTime)
        {
            Vector3 projection = new Vector3(1, 1, 1);
            if (!flyMode)
                projection = new Vector3(1, 0, 1);
            float velocity = movementSpeed * deltaTime;

            if (key == Keys.Space)
            {
                if (flyMode)
                    position += camera.worldUp * velocity * projection;
                else if(velocityY == 0)
                    velocityY -= 10 * deltaTime;
            }
            if (key == Keys.W)
            {
                position += camera.front * velocity * projection;
            }
            if (key == Keys.S)
            {
                position -= camera.front * velocity * projection;
            }
            if (key == Keys.A)
            {
                position -= camera.right * velocity * projection;
            }
            if (key == Keys.D)
            {
                position += camera.right * velocity;
            }
        }

        public void HandleCollisions()
        {
            collider.updateGlobalCollision(position, new Vector3(0, 0, 0));

            IEnumerable<GameObject> list = MainLogic.gameObjects.Where(a => (a.position - position).Length < 10 && (a is WallPrefab || a is FloorPrefab)).OrderBy(a => (a.position - position).Length);
            foreach (GameObject obj in list)
            {
                BoxCollider objCollider;
                if (obj is WallPrefab)
                    objCollider = ((WallPrefab)obj).collision;
                else
                    objCollider = ((FloorPrefab)obj).collision;

                if (collider.CollidesWith(objCollider))
                {
                    Vector3 diff = collider.getDiff(objCollider);
                    position += diff;
                    if (diff.Y > 0)
                        velocityY = 0;
                    collider.updateGlobalCollision(position, new Vector3(0, 0, 0));
                }
            }
        }
    }
}
