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

        // simple objects to border the edges of the window off
        Instance.Level.children.Add(new SolidObject()
        {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(Size.X, 10),
            Color = Color.White,
        });

        Instance.Level.children.Add(new SolidObject()
        {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(10, Size.Y),
            Color = Color.White,
        });

        Instance.Level.children.Add(new SolidObject()
        {
            Position = new Vector2f(Size.X - 10, 0),
            Size = new Vector2f(10, Size.Y),
            Color = Color.White,
        });

        Instance.Level.children.Add(new SolidObject()
        {
            Position = new Vector2f(0, Size.Y - 10),
            Size = new Vector2f(Size.X, 10),
            Color = Color.White,
        });

        // some test rigid objects
        Instance.Level.children.Add(new RigidObject()
        {
            Position = new Vector2f(200, 200),
            Size = new Vector2f(50, 20),
            Color = Color.Blue,
        });

        Size = new Vector2u(800, 600);

        // add the debug menu to the scene
        Instance.Level.children.Add(Instance.DebugMenu);
    }

    public Game()
    {
        // some debugging information
        Instance.Level.children.Add(Instance.DebugText);

        // add the player to the level
        Instance.Level.children.Add(Instance.Player);

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
            "Player Size: " + Instance.Player.Size.ToString(),
            "Player Rotation: " + Instance.Player.Rotation.ToString(),
            "Player Velocity: " + Instance.Player.Velocity.Main.ToString(),
            "Player Movement: " + Instance.Player.Velocity["movement"].ToString(),
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