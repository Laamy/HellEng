using SFML.Graphics;
using SFML.System;
using System;
using System.Linq;
using System.Windows.Forms;

class Bounds
{
    public FloatRect _original; // original bounds
    public Transformable _transformable; // transformable object
    public RectangleShape _rect; // rectangle shape

    public FloatRect? _cache; // cached bounds to avoid recalculating when not needed

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

        _cache = bounds;

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
        set
        {
            _transformable.Position = value;
            _cache = null; // clear the cache to force recalculation
        }
    }

    public Vector2f Size
    {
        get => new Vector2f(_original.Width, _original.Height);
        set
        {
            _original.Width = value.X;
            _original.Height = value.Y;
            _cache = null; // clear the cache to force recalculation
        }
    }

    public Color Color
    {
        get => _rect.FillColor;
        set => _rect.FillColor = value;
    }

    public FloatRect GetGlobalBounds()
    {
        if (_cache == null)
        {
            FloatRect bounds = _transformable.Transform.TransformRect(_original);
            _cache = bounds;
            return bounds;
        }
        else return (FloatRect)_cache;
    }

    public bool Intersects(Bounds other)
    {
        FloatRect thisBounds = GetGlobalBounds();
        FloatRect otherBounds = other.GetGlobalBounds();

        // the FloatRect.Intersects method is not fast enough for our needs
        // so we've setup some custom code to do the same thing but a bit faster
        // btw we dont use math.max and .min cuz they have extra checks and it goes off into other memory
        // which takes time to access and slows down the process

        // Compute the intersection boundaries
        float interLeft = (thisBounds.Left > otherBounds.Left) ? thisBounds.Left : otherBounds.Left;
        float interTop = (thisBounds.Top > otherBounds.Top) ? thisBounds.Top : otherBounds.Top;
        float interRight = (thisBounds.Left + thisBounds.Width < otherBounds.Left + otherBounds.Width) ? thisBounds.Left + thisBounds.Width : otherBounds.Left + otherBounds.Width;
        float interBottom = (thisBounds.Top + thisBounds.Height < otherBounds.Top + otherBounds.Height) ? thisBounds.Top + thisBounds.Height : otherBounds.Top + otherBounds.Height;

        // If the intersection is valid (positive non-zero area), then there is an intersection
        return interLeft < interRight && interTop < interBottom;
    }

    public bool IsInside(FloatRect otherBounds)
    {
        FloatRect thisBounds = GetGlobalBounds();

        // Check if this rectangle is completely inside the other rectangle (make sure to use condoms!)
        return thisBounds.Left >= otherBounds.Left &&
               thisBounds.Top >= otherBounds.Top &&
               thisBounds.Left + thisBounds.Width <= otherBounds.Left + otherBounds.Width &&
               thisBounds.Top + thisBounds.Height <= otherBounds.Top + otherBounds.Height;
    }

    public bool ResolveCollision(Bounds other)
        => ResolveCollision(other, out _);

    public bool ResolveCollision(Bounds other, out bool xAxis, bool adjust = true)
    {
        if (Intersects(other))
        {
            FloatRect thisRotatedBounds = CalculateRotatedBounds();
            FloatRect otherRotatedBounds = other.CalculateRotatedBounds();

            float minX = Math.Max(thisRotatedBounds.Left, otherRotatedBounds.Left);
            float maxX = Math.Min(thisRotatedBounds.Left + thisRotatedBounds.Width, otherRotatedBounds.Left + otherRotatedBounds.Width);

            float minY = Math.Max(thisRotatedBounds.Top, otherRotatedBounds.Top);
            float maxY = Math.Min(thisRotatedBounds.Top + thisRotatedBounds.Height, otherRotatedBounds.Top + otherRotatedBounds.Height);

            float overlapX = maxX - minX;
            float overlapY = maxY - minY;

            // Determine the axis of minimum penetration
            if (overlapX < overlapY)
            {
                if (adjust)
                {
                    // Colliding on the X-axis
                    float pushX = overlapX * (this.Position.X < other.Position.X ? -1 : 1);
                    Position = new Vector2f(Position.X + pushX, Position.Y);
                }

                xAxis = true;
            }
            else
            {
                if (adjust)
                {
                    // Colliding on the Y-axis
                    float pushY = overlapY * (this.Position.Y < other.Position.Y ? -1 : 1);
                    Position = new Vector2f(Position.X, Position.Y + pushY);
                }

                xAxis = false;
            }

            return true;
        }

        xAxis = false;
        return false;
    }

    // Helper method to calculate rotated bounds
    private FloatRect CalculateRotatedBounds()
    {
        float angleRad = MathHelper.ToRadians(Rotation);
        float sinA = (float)Math.Sin(angleRad);
        float cosA = (float)Math.Cos(angleRad);

        // Calculate the rotated points
        Vector2f[] rotatedPoints = new Vector2f[4];
        rotatedPoints[0] = new Vector2f(Position.X, Position.Y);
        rotatedPoints[1] = new Vector2f(Position.X + Size.X * cosA, Position.Y + Size.X * sinA);
        rotatedPoints[2] = new Vector2f(Position.X + Size.X * cosA - Size.Y * sinA, Position.Y + Size.X * sinA + Size.Y * cosA);
        rotatedPoints[3] = new Vector2f(Position.X - Size.Y * sinA, Position.Y + Size.Y * cosA);

        // Find the bounding box of the rotated points
        float left = rotatedPoints.Min(p => p.X);
        float top = rotatedPoints.Min(p => p.Y);
        float right = rotatedPoints.Max(p => p.X);
        float bottom = rotatedPoints.Max(p => p.Y);

        return new FloatRect(left, top, right - left, bottom - top);
    }

    public bool Intersects(Bounds other, out FloatRect by)
        => GetGlobalBounds().Intersects(other.GetGlobalBounds(), out by);

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