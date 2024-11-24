using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;


public class Camera
{

    private Vector3 position;
    private Vector3 front;
    private Vector3 up;
    private Vector3 right;
    private Vector3 worldUp;
    private Quaternion orientation;
    private float movementSpeed;
    private float mouseSensetivity;
    private float fov;

    private void updateCameraVectors()
    {
        Vector3 inputVector = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 rotatedVector = Vector3.Transform(inputVector, orientation);
        front = Vector3.Normalize(rotatedVector);
        right = Vector3.Normalize(Vector3.Cross(front, worldUp));
        up = Vector3.Normalize(Vector3.Cross(right, front));
    }


    public void processKeyboard(Keys key, float deltaTime)
    {
        float velocity = movementSpeed * deltaTime;

        if (key == Keys.W)
        {
            position += front * velocity;
        }
        if (key == Keys.S)
        {
            position -= front * velocity;
        }
        if (key == Keys.A) 
        {
            position -= right * velocity;
        }
        if (key == Keys.D) 
        {
            position += right * velocity;
        }
    }
    public void processMouseMovement(float xoffset, float yoffset)
    {
        xoffset *= mouseSensetivity;
        yoffset *= mouseSensetivity;

        // Создаем кватернионы для поворота вокруг осей Y и X
        Quaternion yawRotation = Quaternion.FromAxisAngle(worldUp, xoffset);
        Quaternion pitchRotation = Quaternion.FromAxisAngle(right, yoffset);

        orientation = yawRotation * orientation * pitchRotation;
        orientation = orientation.Normalized();

        updateCameraVectors();
    }

    public Matrix4 getProjectionMatrix(float aspectRatio)
    {
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspectRatio, 0.1f, 300.0f);

        return projection;
    }

    public Camera(Vector3 _position, Vector3 _up)
    {
        position = _position;
        worldUp = _up;
        front = new Vector3(0.0f, 0.0f, -1.0f);
        movementSpeed = 0.05f;
        mouseSensetivity = 0.1f;
        fov = 90.0f;

        updateCameraVectors();
    }

    public Matrix4 GetMatrix()
    {
        Matrix4 view = Matrix4.LookAt(position, position + front, up);
        return view;
    }

    public Vector3 GetPosition()
    {
        return position;
    }
}
