using OpenTK.Mathematics;

namespace Utils;

public static class InteropExt
{
    public static float[] Flatten(this Matrix4 mat)
    {
        float[] arr = new float[16];

        for (int i = 0; i < 16; i++)
        {
            arr[i] = mat[i/4, i%4];
        }

        return arr;
    }
}
