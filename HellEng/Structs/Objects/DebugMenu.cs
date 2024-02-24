using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

internal class DebugMenu : RawObject
{
    bool init = false;

    // scene expandables dictionary
    RawObject CurrentExpanded = null;

    public override void Draw(RenderWindow e)
    {
        // init done or not?>
        if (!init)
        {
            init = true;

            e.MouseButtonPressed += E_MouseLeft;
        }

        // draw a simple menu on the right side of the window that lists the level objects
        
        // draw the background
        foreach (RawObject obj in Game.Instance.Level.children)
        {
            if (obj == null) continue;

            // background for the object label
            RectangleShape rect = new RectangleShape(new Vector2f(200, 16));
            rect.Position = new SFML.System.Vector2f(e.Size.X - 100, 0 + 16 * Game.Instance.Level.children.IndexOf(obj));
            rect.FillColor = Color.White;
            e.Draw(rect);

            Text text = new Text(obj.GetType().Name, Game.Instance.FontRepos.GetFont("arial"), 16);
            text.Position = new SFML.System.Vector2f(e.Size.X - 100, 0 + 16 * Game.Instance.Level.children.IndexOf(obj));
            text.FillColor = Color.Black;
            e.Draw(text);

            if (CurrentExpanded == obj)
            {
                if (obj is SolidObject)
                {
                    SolidObject solid = (SolidObject)obj;

                    DrawStrAt(e, $"Position: {solid.Position}", new Vector2f(10, e.Size.Y - 10 - (16 * 1)));
                    DrawStrAt(e, $"Size: {solid.Size}", new Vector2f(10, e.Size.Y - 10 - (16 * 2)));
                    DrawStrAt(e, $"Rotation: {solid.Rotation}", new Vector2f(10, e.Size.Y - 10 - (16 * 3)));
                }

                if (obj is SolidText)
                {
                    SolidText textObj = (SolidText)obj;

                    DrawStrAt(e, $"Text: {textObj.Text.Replace("\n", "\\n")}", new Vector2f(10, e.Size.Y - 10 - (16 * 1)));
                    DrawStrAt(e, $"Position: {textObj.Position}", new Vector2f(10, e.Size.Y - 10 - (16 * 2)));
                    DrawStrAt(e, $"Size: {textObj.Size}", new Vector2f(10, e.Size.Y - 10 - (16 * 3)));
                    DrawStrAt(e, $"Rotation: {textObj.Rotation}", new Vector2f(10, e.Size.Y - 10 - (16 * 4)));
                    DrawStrAt(e, $"Font: {textObj.Font}", new Vector2f(10, e.Size.Y - 10 - (16 * 5)));
                    DrawStrAt(e, $"Colour: {textObj.Color}", new Vector2f(10, e.Size.Y - 10 - (16 * 6)));
                }

                if (obj is RigidObject)
                {
                    RigidObject rigid = (RigidObject)obj;

                    DrawStrAt(e, $"Velocity: {rigid.Velocity.Main}", new Vector2f(10, e.Size.Y - 10 - (16 * 4)));
                    DrawStrAt(e, $"AirDrag: {rigid.AirDrag}", new Vector2f(10, e.Size.Y - 10 - (16 * 5)));
                    DrawStrAt(e, $"Colliding: {rigid.Colliding}", new Vector2f(10, e.Size.Y - 10 - (16 * 6)));
                    DrawStrAt(e, $"Grounded: {rigid.Grounded}", new Vector2f(10, e.Size.Y - 10 - (16 * 7)));
                    DrawStrAt(e, $"Gravity: {rigid.Gravity}", new Vector2f(10, e.Size.Y - 10 - (16 * 8)));
                }

                if (obj is LocalPlayer)
                {
                    LocalPlayer localPlayer = (LocalPlayer)obj;

                    DrawStrAt(e, $"MoveSpeed: {localPlayer.MoveSpeed}", new Vector2f(10, e.Size.Y - 10 - (16 * 9)));
                    DrawStrAt(e, $"JumpPower: {localPlayer.JumpPower}", new Vector2f(10, e.Size.Y - 10 - (16 * 10)));
                }
            }
        }
    }

    public void DrawStrAt(RenderWindow e, string str, Vector2f pos)
    {
        Text text = new Text(str, Game.Instance.FontRepos.GetFont("arial"), 16);
        text.Position = pos;
        text.FillColor = Color.Black;
        e.Draw(text);
    }

    private void E_MouseLeft(object sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left)
            return;

        // store mouse pos to the form
        Vector2i pos = new Vector2i(e.X, e.Y);

        // window it was called from
        RenderWindow window = (RenderWindow)sender;

        Console.WriteLine(pos.X + " " + pos.Y);

        // check what object is under the mouse
        foreach (RawObject obj in Game.Instance.Level.children)
        {
            if (obj == null) continue;
            
            // calculate the bounding box of the object label
            FloatRect rect = new FloatRect(window.Size.X - 200, 0 + 16 * Game.Instance.Level.children.IndexOf(obj), 200, 16);

            // check if the mouse is inside the bounding box
            if (rect.Contains(pos.X, pos.Y))
            {
                // if it is, expand the object
                CurrentExpanded = obj;
            }
        }
    }
}