// extension for Vector2f for .Distance

using SFML.System;
using System;

static class Extensions
{
    public static float Distance(this Vector2f a, Vector2f b)
    {
        return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}