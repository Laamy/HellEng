#region Includes

using SFML.Graphics;
using SFML.System;
using System.Diagnostics;

#endregion

internal class Game : GameEngine
{
    public static ClientInstance Instance = new ClientInstance();

    protected override void OnLoad()
    {
        // set the window title
        Title = "HellEngine";

        // set the window size
        Size = new Vector2u(800, 600);
    }

    public Game()
    {
        // some debugging information
        Instance.Level.children.Add(Instance.DebugText);

        // add the player to the level
        Instance.Level.children.Add(Instance.Player);

        // simple floor for the level
        Instance.Level.children.Add(new SolidObject()
        {
            Position = new Vector2f(0, 500),
            Size = new Vector2f(800, 20),
            Color = Color.White,
        });

        // we've finished so start the app
        Start();
    }

    protected override void OnUpdate()
    {
        Instance.Level.Update(this); // update game logic

        // update the debug text
        Instance.SetDebugText(new string[] {
            //"FPS: " + (1 / DeltaTime).ToString("0"),
            "Player Position: " + Instance.Player.Position.ToString(),
        });
    }

    protected override void OnDraw(RenderWindow ctx)
    {
        ctx.Clear(Color.Red); // clear buffer ready for next frame
        ctx.DispatchEvents(); // handle window events

        Instance.Level.Draw(ctx); // draw scene

        ctx.Display(); // swap buffers
    }
}