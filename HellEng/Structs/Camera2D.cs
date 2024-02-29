using SFML.Graphics;
using SFML.System;

internal class Camera2D : RawObject
{
    // SFML camera 2d thats movable and zoomable
    public View View { get; } = new View(new FloatRect(0, 0, 1920, 1080));

    public float Zoom = 1.0f; // zoom level
    public float ZoomSpeed = 0.1f; // zoom speed
    public float MoveSpeed = 0.1f; // move speed

    public void Goto(Vector2f position)
    {
        View.Center = position;
    }

    public override void Draw(RenderWindow e)
    {
        View.Zoom(Zoom); // apply zoom
        e.SetView(View); // apply view to window
    }

    public override void Update(Game game)
    {
        // update view viewport
        View.Size = new Vector2f(game.Window.Size.X, game.Window.Size.Y);
    }
}