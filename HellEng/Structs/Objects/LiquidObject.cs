using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

internal class LiquidObject : RawObject
{
    private ushort prevId = 0; // the last clipping rectangle id used

    public  ConcurrentDictionary<int, FloatRect> ClippingAreas { get; private set; }
        = new ConcurrentDictionary<int, FloatRect>(); // the clipping areas for the liquid object indexed by id

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

        ClippingAreas[prevId++] = clipRect; // add to the clipping areas

        Invalidate(); // invalidate the object cuz we need to redraw

        return prevId;
    }

    public void InvalidateSprite()
    {
        if (sprite != null)
            sprite.Dispose(); // dispose of the sprite

        sprite = new Sprite(renderTexture.Texture); // create a new sprite
        sprite.Position = Position;
    }

    private void EraseRegion(FloatRect area) // this works but no way its fast so TODO: optimzie this
    {
        if (area == null)
            return;

        // Get the texture data from the RenderTexture
        Image textureData = renderTexture.Texture.CopyToImage();

        // Calculate the integer rectangle for the specified area
        IntRect intRect = new IntRect(
            (int)area.Left,
            (int)area.Top,
            (int)area.Width,
            (int)area.Height
        );

        // Erase the pixels in the specified region by setting them to transparent
        for (int y = intRect.Top; y < intRect.Top + intRect.Height; ++y)
        {
            for (int x = intRect.Left; x < intRect.Left + intRect.Width; ++x)
            {
                textureData.SetPixel((uint)x, (uint)y, Color.Transparent);
            }
        }

        // Update the texture with the new data
        renderTexture.Texture.Update(textureData);
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