using SFML.Graphics;
using SFML.System;
using SFML.Window;

using System;

internal class MiniSubObject : GroupObject
{
    // we're inheriting the group object class so we can have seats & other things for the mini-submarine
    public LocalPlayer Driver = null; // the current parent of the mini-submarine
    public InteractObject Interact = null; // the interactable object for the mini-submarine

    public Vector2f DriverOffset = new Vector2f(0, 0); // the offset of the driver from the top left of the mini-submarine

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

        // debug interactable object
        Interact = new InteractObject()
        {
            Position = Extensions.CacheObject,
            Keybind = Keyboard.Key.F,
            Range = 150,
            OnInteract = (object sender, EventArgs e) =>
            {
                Interact.Visible = false;
                Console.WriteLine("Entered minisub controls!");

                // set the driver to the player
                Driver = Game.Instance.Player;

                // lock the player's movement
                Driver.LockMove = true;

                // set the driver offset to the sub - player pos
                DriverOffset = Driver.Position - Position;
            }
        };

        Game.Instance.Level.Children.Add(Interact);
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

                // escape (eject action)
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    Interact.Visible = true; // show the interactable object

                    Driver.LockMove = false; // unlock the player's movement
                    Driver = null; // remove the driver

                    return;
                }

                // up down (movement Y action)
                if (Keyboard.IsKeyPressed(Keyboard.Key.W)) move.Y -= UpSpeed;
                if (Keyboard.IsKeyPressed(Keyboard.Key.S)) move.Y += UpSpeed;

                // left right (movement X action)
                if (Keyboard.IsKeyPressed(Keyboard.Key.A)) move.X -= SideSpeed;
                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) move.X += SideSpeed;

                // sprint (sprint action)
                if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) move *= SprintScale;

                Position += move;
            }

            //we're driving soi update the player pos to be sub + offset
            Driver.Position = Position + DriverOffset;
        }

        if (Interact != null)
            Interact.Position = Position + new Vector2f(50, 50);

        base.Update(game);
    }
}