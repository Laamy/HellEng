using SFML.Graphics;
using SFML.System;

class Bounds
{
    private FloatRect _original; // original bounds
    private Transformable _transformable; // transformable object
    private RectangleShape _rect; // rectangle shape

    public Bounds(FloatRect rect)
    {
        // store the original bounds
        _original = rect;

        // create a transformable object
        _transformable = new Transformable();
        _transformable.Position = new Vector2f(rect.Left, rect.Top);
        _transformable.Origin = new Vector2f(rect.Width / 2, rect.Height / 2);

        // get the bounds of the transformable object
        FloatRect bounds = GetGlobalBounds();

        // create a rectangle shape
        _rect = new RectangleShape(new Vector2f(bounds.Width, bounds.Height));
    }

    public float Rotation
    {
        get => _transformable.Rotation;
        set => _transformable.Rotation = value;
    }

    public Vector2f Position
    {
        get => _transformable.Position;
        set => _transformable.Position = value;
    }

    public Vector2f Size
    {
        get => new Vector2f(_original.Width, _original.Height);
        set
        {
            _original.Width = value.X;
            _original.Height = value.Y;
        }
    }

    public Color Color
    {
        get => _rect.FillColor;
        set => _rect.FillColor = value;
    }

    public FloatRect GetGlobalBounds()
        => _transformable.Transform.TransformRect(_original);

    public bool Intersects(Bounds other)
        => GetGlobalBounds().Intersects(other.GetGlobalBounds());

    public void Draw(RenderWindow ctx, Color? fill = null)
    {
        // render the rectangle shape with the transformable component
        _rect.Position = Position;
        _rect.Size = Size;
        _rect.Rotation = Rotation;

        // set the fill colour if specified
        if (fill != null)
            _rect.FillColor = fill.Value;

        // draw the rectangle shape
        ctx.Draw(_rect);
    }
}