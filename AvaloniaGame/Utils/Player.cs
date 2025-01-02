using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.GameLogic;
using AvaloniaGame.GameLogic.Collider;

namespace AvaloniaGame.Utils
{
    public class Player : GameObject
    {
        private float movementSpeed;
        private float gravity;
        private float velocityY;
        private Vector3 cameraPosition;
        public Camera camera;
        private float stepTime = 0;

        bool flyMode;
        Random soundEvent = new Random();

        public BoxCollider collider { get; private set; }

        AudioObject AudioPlayer = new AudioObject();
        AudioObject AudioEvents = new AudioObject();

        // HACK: уберите детей от экрана
        struct GameInputs
        {
            public bool W;
            public bool A;
            public bool S;
            public bool D;
            public bool F;
        }

        GameInputs gameInputs = new();
        Point? lastPointerPos = new();
        Point pointerDelta = new();

        public Player(GL gl, Vector3 _position, Vector3 _size, float _cameraHeight, float _speed, float _gravity) : base(gl)
        {
            position = _position;
            movementSpeed = _speed;
            gravity = _gravity;
            velocityY = 0;
            flyMode = false;

            cameraPosition = new Vector3(0, _cameraHeight, 0);
            camera = new Camera(gl, cameraPosition, new Vector3(0.0f, 1.0f, 0.0f));

            collider = new BoxCollider(new List<Vector3>
            {
                new (-_size.X/2, 0,        -_size.Z/2),
                new (-_size.X/2, 0,         _size.Z/2),
                new (-_size.X/2, _size.Y,  -_size.Z/2),
                new (-_size.X/2, _size.Y,   _size.Z/2),
                new ( _size.X/2,  0f,      -_size.Z/2),
                new ( _size.X/2,  0f,       _size.Z/2),
                new ( _size.X/2,  _size.Y, -_size.Z/2),
                new ( _size.X/2,  _size.Y,  _size.Z/2)
            });

            AudioPlayer.LoadAudio("Steps");
            AudioEvents.LoadAudio("Events");
        }

        public void PointerMovedHandler(object? sender, PointerEventArgs e)
        {
            var newPos = e.GetPosition(sender as Control);

            if (lastPointerPos is not null)
            {
                pointerDelta = newPos - lastPointerPos.Value;
            }
            //Console.WriteLine($"X: {pointerDelta.X}\tY: {pointerDelta.Y}");
            lastPointerPos = newPos;
        }

        public void KeyUpHandler(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    gameInputs.W = false;
                    e.Handled = true;
                    break;
                case Key.A:
                    gameInputs.A = false;
                    e.Handled = true;
                    break;
                case Key.S:
                    gameInputs.S = false;
                    e.Handled = true;
                    break;
                case Key.D:
                    gameInputs.D = false;
                    e.Handled = true;
                    break;
                case Key.F:
                    gameInputs.F = false;
                    e.Handled = true;
                    break;
            };
        }

        public void KeyDownHandler(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    gameInputs.W = true;
                    e.Handled = true;
                    break;
                case Key.A:
                    gameInputs.A = true;
                    e.Handled = true;
                    break;
                case Key.S:
                    gameInputs.S = true;
                    e.Handled = true;
                    break;
                case Key.D:
                    gameInputs.D = true;
                    e.Handled = true;
                    break;
                case Key.F:
                    gameInputs.F = true;
                    e.Handled = true;
                    break;
            }
        }

        public override void Update(float deltaTime)
        {
            var inputVelocity = Vector2.Zero;

            if (gameInputs.W)
            {
                inputVelocity.Y = 1f;
            }
            if (gameInputs.A)
            {
                inputVelocity.X = -1f;
            }
            if (gameInputs.S)
            {
                inputVelocity.Y = -1f;
            }
            if (gameInputs.D)
            {
                inputVelocity.X = 1f;
            }

            if ( gameInputs.F)
            {
                if(!flyMode)
                {
                    velocityY = 0f;
                }
                flyMode = !flyMode;
            }

            camera.processMouseMovement((float)pointerDelta.X, (float)pointerDelta.Y);
            // Костыль чтобы не было дрифта камеры если не двигать мышью
            pointerDelta = pointerDelta.WithX(0f);
            pointerDelta = pointerDelta.WithY(0f);

            /*if (keyboard.IsKeyDown(Keys.Space))
                ProcessKeyboard(Keys.Space, deltaTime);*/

            if (inputVelocity != Vector2.Zero)
            {
                stepTime += deltaTime;
                if (stepTime > 0.5)
                {
                    AudioPlayer.PlayAudio(looped:false);
                    stepTime = 0;
                }
                HandleMovement(inputVelocity, deltaTime);
            }


            if (soundEvent.Next(0, 2000) == 666)
            {
                AudioEvents.PlayAudio(looped: false);
            }

            if (!flyMode)
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

        /*
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
        */

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

        public override void Start(GL gl)
        {
            throw new NotImplementedException();
        }
    }
}
