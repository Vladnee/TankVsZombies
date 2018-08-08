using UnityEngine;

public static class VectorExtensions
{
    public static Quaternion LookRotation2D(this Vector2 at, float shift = -90)
    {
        float rot_z = Mathf.Atan2(at.y, at.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z + shift);
    }
}