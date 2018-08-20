using UnityEngine;

public static class VectorExtensions
{
    public static Quaternion LookRotation2D(this Vector2 at, float shift = -90)
    {
        float rot_z = Mathf.Atan2(at.y, at.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z + shift);
    }

    public static Vector2 PerpendicularClockwise(this Vector2 vector)
    {
        return new Vector2(vector.y, -vector.x).normalized;
    }

    public static Vector2 PerpendicularCounterClockwise(this Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x).normalized;
    }
}