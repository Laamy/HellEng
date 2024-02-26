#region Includes

using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

#endregion

internal class SolidObject : RawObject
{
    public SolidObject()
    {
        // create the tags list
        Tags = new List<string>();

        // create the bounds with temp FloatRect values
        Bounds = new Bounds(new FloatRect(0, 0, 0, 0));
    }

    // base
    public Bounds Bounds { get; set; }
    public List<string> Tags;

    public Vector2f Position
    {
        get => Bounds.Position;
        set => Bounds.Position = value;
    }

    public Vector2f OffsetPosition
    {
        get => Bounds.OffsetPosition;
        set => Bounds.OffsetPosition = value;
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