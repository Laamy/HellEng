using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

internal class LiquidObject : RawObject
{
    private ushort prevId = 0; // the last clipping rectangle id used

    public  Dictionary<int, RectangleShape> ClippingAreas { get; set; }
        = new Dictionary<int, RectangleShape>(); // the clipping areas for the liquid object indexed by id

    private RenderTexture renderTexture; // the render texture to draw the liquid object to
    private Sprite sprite; // the sprite used to draw the liquid object textures

    private RenderStates renderState = RenderStates.Default;

    public LiquidObject()
    {
        Tags = new List<string>(); // create the tags list

        Bounds = new Bounds(new FloatRect(0, 0, 1, 1)); // create the bounds with temp FloatRect values

        renderTexture = new RenderTexture((uint)Size.X, (uint)Size.Y); // create the render texture
        renderTexture.Clear(Color); // clear the render texture
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
        set
        {
            Bounds.Size = value;

            renderTexture = new RenderTexture((uint)Size.X, (uint)Size.Y); // create the render texture
            Invalidate(); // invalidate the object cuz we need to redraw
        }
    }

    public Color Color
    {
        get => Bounds.Color;
        set => Bounds.Color = value;
    }

    public int AddClippingArea(Vector2f position, Vector2f size)
    {
        // create rectangleshape
        FloatRect clipRect = new FloatRect(new Vector2f(position.X, position.Y), new Vector2f(size.X, size.Y));

        // create new drawable for clipping rectangle
        RectangleShape clip = new RectangleShape()
        {
            Position = position,
            Size = size,
            FillColor = Color.Transparent,
        };

        ClippingAreas[prevId++] = clip; // add to the clipping areas

        Invalidate(); // invalidate the object cuz we need to redraw

        return prevId;
    }

    public void InvalidateSprite()
    {
        if (sprite != null)
            sprite.Dispose();

        sprite = new Sprite(renderTexture.Texture); // create the sprite
        sprite.Position = Position; // set the position
    }

    private void EraseRegion(RectangleShape area) // this works but no way its fast so TODO: optimzie this
    {
        if (area == null)
            return;

        renderTexture.Draw(area, new RenderStates(BlendMode.None));
    }

    public void Invalidate()
    {
        renderTexture.Clear(Color); // object invalidated so lets clear and reapply the clippings

        foreach (var clip in ClippingAreas)
            EraseRegion(clip.Value); // erase the region

        InvalidateSprite(); // update sprite
    }

    public override void Draw(RenderWindow e)
    {
        // render the rectangle shape with the transformable component
        //Bounds._rect.Position = Position;
        //Bounds._rect.Size = Size;
        //Bounds._rect.Rotation = Rotation;

        // draw the rectangle shape
        //e.Draw(Bounds._rect);

        sprite.Draw(e, renderState); // draw the sprite which contains the texture
    }
}