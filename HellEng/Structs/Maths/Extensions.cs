using SFML.System;

using System;

static class Extensions
{
    public static Vector2f CacheObject = new Vector2f(-10000, -10000);

    /// <summary>
    /// Returns the distance between two Vector2f's
    /// </summary>
    public static float Distance(this Vector2f _this, Vector2f b)
    {
        return (float)Math.Sqrt(Math.Pow(_this.X - b.X, 2) + Math.Pow(_this.Y - b.Y, 2));
    }

    /// <summary>
    /// ToCameraSpace method
    /// </summary>
    public static Vector2f ToCameraSpace(this Vector2f _this, Camera2D camera)
    {
        return new Vector2f(_this.X + (camera.View.Center.X - (camera.View.Size.X / 2)), _this.Y + (camera.View.Center.Y - (camera.View.Size.Y / 2)));
    }
}