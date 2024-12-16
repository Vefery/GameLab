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
    public class Player : GameObject
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

            Vector2 inputVelocity = Vector2.Zero;

            /*if (keyboard.IsKeyDown(Keys.Space))
                ProcessKeyboard(Keys.Space, deltaTime);*/
            if (keyboard.IsKeyDown(Keys.W))
                inputVelocity.Y = 1f;
            if (keyboard.IsKeyDown(Keys.S))
                inputVelocity.Y = -1f;
            if (keyboard.IsKeyDown(Keys.A))
                inputVelocity.X = -1f;
            if (keyboard.IsKeyDown(Keys.D))
                inputVelocity.X = 1f;

            if (inputVelocity != Vector2.Zero)
                HandleMovement(inputVelocity, deltaTime);

            if(!flyMode)
            {
                velocityY += gravity * deltaTime;
                position -= new Vector3(0, velocityY, 0);
            }
            HandleCollisions();
            camera.position = position + cameraPosition;
        }

        public void HandleMovement(Vector2 inputVelocity, float deltaTime)
        {
            Vector3 projection = new(1, 1, 1);
            if (!flyMode)
                projection = new(1, 0, 1);

            position += ((camera.right * inputVelocity.X + camera.front * inputVelocity.Y) * projection).Normalized() * movementSpeed * deltaTime;
        }
        public void ProcessKeyboard(Keys key, float deltaTime)
        {
            Vector3 projection = new(1, 1, 1);
            if (!flyMode)
                projection = new(1, 0, 1);
            float velocity = movementSpeed * deltaTime;

            if (key == Keys.Space)
            {
                if (flyMode)
                    position += camera.worldUp * velocity * projection;
                else if(velocityY == 0)
                    velocityY -= 10 * deltaTime;
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
