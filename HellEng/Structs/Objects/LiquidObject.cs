using SFML.Graphics;
using SFML.System;

using System.Collections.Generic;

internal class LiquidObject : RawObject
{
    public LiquidObject()
    {
        // create the tags list
        Tags = new List<string>();

        // create the bounds with temp FloatRect values
        Bounds = new Bounds(new FloatRect(0, 0, 0, 0));
    }
    public float Friction { get; internal set; } = 0.7f;

    // base
    public Bounds Bounds { get; set; }
    public List<string> Tags;

    public Vector2f Position
    {
        get => Bounds.Position;
        set => Bounds.Position = value;
    }

    public float Rotation
    {
        get => Bounds.Rotation;
        set => Bounds.Rotation = value;
    }

    public Vector2f Size
    {
        get => Bounds.Size;
        set => Bounds.Size = value;
    }

    public Color Color
    {
        get => Bounds.Color;
        set => Bounds.Color = value;
    }

    public override void Draw(RenderWindow e)
    {
        Bounds.Draw(e);
    }
}