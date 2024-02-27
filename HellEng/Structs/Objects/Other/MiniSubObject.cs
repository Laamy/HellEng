using SFML.Graphics;
using SFML.System;
using SFML.Window;

internal class MiniSubObject : GroupObject
{
    // we're inheriting the group object class so we can have seats & other things for the mini-submarine
    public RigidObject Driver = null; // the current parent of the mini-submarine

    public float UpSpeed = 2;
    public float SideSpeed = 2;
    public float SprintScale = 1.7f;

    public MiniSubObject()
    {
        // add some white borders to the air bubble
        Add(Game.Instance.Level, new SolidObject()
        {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(100, 10),
            Color = Color.White,
        });

        Add(Game.Instance.Level, new SolidObject()
        {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(10, 100),
            Color = Color.White,
        });

        Add(Game.Instance.Level, new SolidObject()
        {
            Position = new Vector2f(90, 0),
            Size = new Vector2f(10, 100),
            Color = Color.White,
        });

        Add(Game.Instance.Level, new SolidObject()
        {
            Position = new Vector2f(0, 95),
            Size = new Vector2f(50, 5),
            Color = Color.White,
        });
    }

    public override void Draw(RenderWindow e)
    {
        base.Draw(e);
    }

    public override void Update(Game game)
    {
        if (Driver != null)
        {
            if (game.IsFocused)
            {
                Vector2f move = new Vector2f(0, 0);

                // up down
                if (Keyboard.IsKeyPressed(Keyboard.Key.W)) move.Y -= UpSpeed;
                if (Keyboard.IsKeyPressed(Keyboard.Key.S)) move.Y += UpSpeed;

                // left right
                if (Keyboard.IsKeyPressed(Keyboard.Key.A)) move.X -= SideSpeed;
                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) move.X += SideSpeed;

                // sprint
                if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) move *= SprintScale;

                Position += move;
            }
        }

        base.Update(game);
    }
}