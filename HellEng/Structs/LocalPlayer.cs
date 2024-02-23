using SFML.Graphics;
using SFML.System;
using SFML.Window;

internal class LocalPlayer : SolidObject
{
    public LocalPlayer()
    {
        Position = new Vector2f(100, 100); // start pos
        Size = new Vector2f(50, 50); // test size
        Color = Color.Green; // test colour
        //Rotation = 45; // test rotation
    }

    public override void Update(Game game)
    {
        float speed = 5.0f;

        // calculate a movement vector
        Vector2f move = new Vector2f(0, 0);

        if (game.IsFocused)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) move.Y -= speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) move.Y += speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) move.X -= speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) move.X += speed;

            Position += move; // temp set (might do smth with this later)
        }

        // check if the player is colliding with anything
        foreach (Object obj in Game.Instance.Level.children)
        {
            if (!(obj is SolidObject)) continue; // skip if not a solid object
            if (obj == null) continue; // skip if null

            SolidObject solid = (SolidObject)obj;

            if (solid == this) continue; // skip if it's the player
            if (!solid.Bounds.Intersects(this.Bounds)) continue; // skip if not colliding

            // test solution, we'll calculate the exact distance to move later and in what direction
            this.Position -= move;
        }
    }
}