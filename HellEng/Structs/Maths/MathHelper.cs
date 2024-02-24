public static class MathHelper
{
    public const float Pi = 3.141f;
    public const float TwoPi = 2.0f * Pi;

    public static float ToRadians(float degrees)
    {
        return degrees * (Pi / 180.0f);
    }
}