﻿#region Includes

using SFML.Graphics;
using SFML.System;
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
            GroupObject airBubble = new GroupObject()
            {
                Position = ocean.Position + new Vector2f(100, 100),
                Tags = new List<string>(new string[] { "mini-ship" }),
            };

            // add some white borders to the air bubble
            airBubble.Children.Add(new SolidObject()
            {
                Position = new Vector2f(0, 0),
                Size = new Vector2f(100, 10),
                Color = Color.White,
            });

            airBubble.Children.Add(new SolidObject()
            {
                Position = new Vector2f(0, 0),
                Size = new Vector2f(10, 100),
                Color = Color.White,
            });

            airBubble.Children.Add(new SolidObject()
            {
                Position = new Vector2f(90, 0),
                Size = new Vector2f(10, 100),
                Color = Color.White,
            });

            airBubble.Children.Add(new SolidObject()
            {
                Position = new Vector2f(0, 95),
                Size = new Vector2f(50, 5),
                Color = Color.White,
            });

            Instance.Level.Children.Add(airBubble);
        }

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

        //move the air bubble to the side 1 unit, if its at x=200 we move it to x=100
        SolidObject airBubble = (SolidObject)Instance.Level.ByTag("mini-ship");

        if (airBubble != null)
        {
            if (airBubble.Position.X > 150)
            {
                airBubble.Position = new Vector2f(100, airBubble.Position.Y);
            }
            else
            {
                airBubble.Position = new Vector2f(airBubble.Position.X + 1, airBubble.Position.Y);
            }

            LiquidObject ocean = (LiquidObject)Instance.Level.ByTag("ocean");

            if (ocean != null)
            {
                // get cvlipping bounds
                ocean.ClippingAreas.Clear();

                ocean.AddClippingArea(
                    new Vector2f(airBubble.Position.X, 100),
                    new Vector2f(100, 100)
                );
            }
        }

        // update the debug text
        //Instance.SetDebugText(new string[] {
        //    //"FPS: " + (1 / DeltaTime).ToString("0"),
        //    "Player Position: " + Instance.Player.Position.ToString(),
        //    "Player Size: " + Instance.Player.Size.ToString(),
        //    "Player Rotation: " + Instance.Player.Rotation.ToString(),
        //    "Player Velocity: " + Instance.Player.Velocity.Main.ToString(),
        //    "Player Movement: " + Instance.Player.Velocity["movement"].ToString(),
        //});
    }

    public static Color DarkSkyBlue = new Color(0, 0, 128);

    protected override void OnDraw(RenderWindow ctx)
    {
        ctx.Clear(Color.Red); // clear buffer ready for next frame
        ctx.DispatchEvents(); // handle window events

        Instance.Level.Draw(ctx); // draw scene

        ctx.Display(); // swap buffers
    }
}