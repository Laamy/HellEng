﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;

using System;

internal class LocalPlayer : RigidObject
{
    public LocalPlayer()
    {
        Position = new Vector2f(100, 100); // start pos
        Size = new Vector2f(32, 32); // test size
        Color = Color.Green; // test colour
        Rotation = 0; // test rotation
    }

    public float MoveSpeed = 3.0f; // speed of the player
    public float JumpPower = 20.0f; // jump power
    public float SwimUpSpeed = 5.0f; // swim up speed
    public float SprintScale = 1.5f; // sprint scale (works in liquids)

    public bool LockMove = false; // lock movement of the object

    public override void Update(Game game)
    {
        if (LockMove)
        {
            base.Update(game);
            return; // if we are locked, don't update
        }

        // calculate a movement vector
        Velocity["movement"] = new Vector2f(0, Math.Max(0, Velocity["movement"].Y)); // reset the movement vector everytime we want to update input
        Vector2f move = new Vector2f(0, 0);

        if (game.IsFocused)
        {
            // jump
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                if (InWater)
                {
                    move.Y -= SwimUpSpeed;
                }
                else if (Grounded)
                {
                    move.Y -= JumpPower;
                    Grounded = false;
                }
            }

            // disgarded
            //if (Keyboard.IsKeyPressed(Keyboard.Key.S)) move.Y += speed;

            // left right
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) move.X -= MoveSpeed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) move.X += MoveSpeed;

            // sprint
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) move.X *= SprintScale;

            Velocity["movement"] += move;
        }

        // lets adjust the camera2d pos to the player
        Camera2D camera = (Camera2D)Game.Instance.Level.ByClass<Camera2D>()[0];

        camera.Goto(Position + OffsetPosition);

        base.Update(game);
    }
}