#region Includes

using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

internal class Game : GameEngine
{
    public static ClientInstance Instance = new ClientInstance();

    protected override void OnLoad()
    {
        // set the window title
        Title = "HellEngine";

        // simple objects to border the edges of the window off
        Instance.Level.Children.Add(new SolidObject()
        {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(Size.X, 10),
            Color = Color.White,
        });

        Instance.Level.Children.Add(new SolidObject()
        {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(10, Size.Y),
            Color = Color.White,
        });

        Instance.Level.Children.Add(new SolidObject()
        {
            Position = new Vector2f(Size.X - 10, 0),
            Size = new Vector2f(10, Size.Y),
            Color = Color.White,
        });

        Instance.Level.Children.Add(new SolidObject()
        {
            Position = new Vector2f(0, Size.Y - 10),
            Size = new Vector2f(Size.X, 10),
            Color = Color.White,
        });

        // create some white land objects & caves
        Instance.Level.Children.Add(new SolidObject()
        {
            Position = new Vector2f(0, Size.Y / 2 - 10),
            Size = new Vector2f(Size.X / 2, 40),
            Color = Color.White,
        });

        LiquidObject ocean = new LiquidObject()
        {
            Position = new Vector2f(0, Size.Y / 2),
            Size = new Vector2f(Size.X, Size.Y / 2),
            Color = new Color(0, 0, 255, 128),
            Tags = new List<string>(new string[] { "ocean" }),
        };

        ocean.AddClippingArea(new Vector2f(100, 100), new Vector2f(100, 100));
        ocean.Invalidate();

        {
            // we've created an air bubble in the water so lets give it some borders
            //MiniSubObject miniShip = new MiniSubObject()
            //{
            //    Position = ocean.Position + new Vector2f(100, 100),
            //    Tags = new List<string>(new string[] { "mini-ship" }),
            //    Driver = Instance.Player,
            //};

            //Instance.Level.Children.Add(miniShip);
        }

        // debug interactable object
        InteractObject interact = new InteractObject()
        {
            Position = new Vector2f(300, 250),
            Keybind = Keyboard.Key.F,
            Range = 100,
        };

        interact.OnInteract += (object sender, EventArgs e) =>
        {
            Debug.Log("Interacted with object");
        };

        Instance.Level.Children.Add(interact);

        // cover the boittom half of map as WaterObject
        Instance.Level.Children.Add(ocean);

        // some test rigid objects
        //for (int x = 0; x < 8; x++)
        //{
        //    for (int y = 0; y < 8; y++)
        //    {
        //        Instance.Level.children.Add(new RigidObject()
        //        {
        //            Position = new Vector2f(100 + (x * 4), 400 - (y * 4)),
        //            Size = new Vector2f(4, 4),
        //            Color = new Color((byte)(x * 16), (byte)(y * 16), 255),
        //        });
        //    }
        //}

        Size = new Vector2u(800, 600);

        // add the debug menu to the scene
        Instance.Level.Children.Add(Instance.DebugMenu);
    }

    public Game()
    {
        // some debugging information
        //Instance.Level.children.Add(Instance.DebugText);

        // add the player to the level
        Instance.Level.Children.Add(Instance.Player);

        // we've finished so start the app
        Start();
    }

    protected override void OnUpdate()
    {
        Instance.Level.Update(this); // update game logic

        LiquidObject ocean = (LiquidObject)Instance.Level.ByTag("ocean");
        GroupObject ship = (GroupObject)Instance.Level.ByTag("mini-ship");

        // move the ship over by a bit then updating the clipping rect in the ocean
        if (ship != null)
        {
            ocean.ClippingAreas.Clear();

            Vector2f mirroredUp = ship.Position;

            // morror up to down
            mirroredUp.Y = Size.Y - mirroredUp.Y - 100;

            // create & add new clipping area based on ship dimensions (100,100 as shape)
            ocean.AddClippingArea(mirroredUp, new Vector2f(100, 100));
        }
    }

    public static Color skyblue = new Color(135, 206, 235);

    protected override void OnDraw(RenderWindow ctx)
    {
        ctx.Clear(skyblue); // clear buffer ready for next frame
        ctx.DispatchEvents(); // handle window events

        Instance.Level.Draw(ctx); // draw scene

        ctx.Display(); // swap buffers
    }
}