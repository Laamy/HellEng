using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Runtime.InteropServices;

internal class InteractObject : RawObject
{
    public Vector2f Position { get; set; } // Position of the object
    public Keyboard.Key Keybind { get; set; } // Keybind to interact with the object
    public float Range { get; set; } // Range of the object

    public EventHandler<EventArgs> OnInteract; // Event to call when the object is interacted with

    private bool pressed = false;

    public int Size = 16;
    public bool Visible = true;

    public override void Draw(RenderWindow e)
    {
        // get the localplayer and check if its within range
        LocalPlayer player = (LocalPlayer)Game.Instance.Level.ByClass<LocalPlayer>()[0];

        if (Visible && player.Position.Distance(Position) <= Range)
        {
            // draw the interaction menu at object position
            Vector2f pos = Position;
            pos.Y -= Size * 2;
            pos.X -= Size * 2;

            // round keybind background
            CircleShape background = new CircleShape(Size / 1.2f)
            {
                Position = pos,
                FillColor = new Color(0, 0, 0, 150),
                OutlineThickness = 2,
                OutlineColor = Color.Black
            };

            // draw the background
            e.Draw(background);

            // center and draw text to the circle shape
            Text text = new Text(Keybind.ToString(), Game.Instance.FontRepos.GetFont("arial"), (uint)Size);

            // get the bounds of the circle
            FloatRect bounds = background.GetGlobalBounds();

            // set the origin of the text to the center of the circle
            text.Position = background.Position + new Vector2f(background.Radius / 1.5f, background.Radius / 2.6f); // draws in the wrong spot

            // draw the text
            e.Draw(text);
        }

        base.Draw(e);
    }

    public override void Update(Game game)
    {
        if (Visible && game.IsFocused)
        {
            bool BindHeld = Keyboard.IsKeyPressed(Keybind);

            if (BindHeld && !pressed)
            {
                // get localplayer and check if its within range
                LocalPlayer player = (LocalPlayer)Game.Instance.Level.ByClass<LocalPlayer>()[0];

                if (player.Position.Distance(Position) <= Range)
                {
                    // its within range and key pressed so lets trigger
                    if (OnInteract != null)
                    {
                        OnInteract(this, new EventArgs());
                    }
                }
            }

            pressed = BindHeld;
        }

        base.Update(game);
    }
}