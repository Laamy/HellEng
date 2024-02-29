using SFML.Graphics;
using SFML.System;
using SFML.Window;

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
        foreach (RawObject obj in Game.Instance.Level.Children)
        {
            if (obj == null) continue;

            // background for the object label
            RectangleShape rect = new RectangleShape(new Vector2f(200, FONT_SIZE));
            rect.Position = new SFML.System.Vector2f(e.Size.X - 150, 0 + FONT_SIZE * Game.Instance.Level.Children.IndexOf(obj));
            rect.FillColor = Color.White;
            e.Draw(rect);

            Text text = new Text(obj.GetType().Name, Game.Instance.FontRepos.GetFont("arial"), FONT_SIZE);
            text.Position = new SFML.System.Vector2f(e.Size.X - 150, 0 + FONT_SIZE * Game.Instance.Level.Children.IndexOf(obj));
            text.FillColor = Color.Black;
            e.Draw(text);

            if (CurrentExpanded == obj)
            {
                if (obj is SolidObject)
                {
                    SolidObject solid = (SolidObject)obj;

                    DrawStrAt(e, $"OffsetPosition: {solid.OffsetPosition}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 1)), Color.Black);
                    DrawStrAt(e, $"Position: {solid.Position}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 2)), Color.Black);
                    DrawStrAt(e, $"Size: {solid.Size}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 3)), Color.Black);
                    DrawStrAt(e, $"Rotation: {solid.Rotation}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 4)), Color.Black);
                }

                if (obj is SolidText)
                {
                    SolidText textObj = (SolidText)obj;

                    DrawStrAt(e, $"Text: {textObj.Text.Replace("\n", "\\n")}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 1)), Color.Black);
                    DrawStrAt(e, $"Position: {textObj.Position}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 2)), Color.Black);
                    DrawStrAt(e, $"Size: {textObj.Size}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 3)), Color.Black);
                    DrawStrAt(e, $"Rotation: {textObj.Rotation}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 4)), Color.Black);
                    DrawStrAt(e, $"Font: {textObj.Font}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 5)), Color.Black);
                    DrawStrAt(e, $"Colour: {textObj.Color}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 6)), Color.Black);
                }

                if (obj is RigidObject)
                {
                    RigidObject rigid = (RigidObject)obj;

                    DrawStrAt(e, $"Velocity: {rigid.Velocity.Main}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 5)), Color.Black);
                    DrawStrAt(e, $"AirDrag: {rigid.AirFriction}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 6)), Color.Black);
                    DrawStrAt(e, $"Colliding: {rigid.Colliding}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 7)), Color.Black);
                    DrawStrAt(e, $"Grounded: {rigid.Grounded}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 8)), Color.Black);
                    DrawStrAt(e, $"InWater: {rigid.InWater}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 9)), Color.Black);
                    DrawStrAt(e, $"Gravity: {rigid.Gravity}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 10)), Color.Black);
                }

                if (obj is LocalPlayer)
                {
                    LocalPlayer localPlayer = (LocalPlayer)obj;

                    DrawStrAt(e, $"MoveSpeed: {localPlayer.MoveSpeed}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 11)), Color.Black);
                    DrawStrAt(e, $"JumpPower: {localPlayer.JumpPower}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 12)), Color.Black);
                    DrawStrAt(e, $"Movement: {localPlayer.Velocity["movement"]}", new Vector2f(10, e.Size.Y - 10 - (FONT_SIZE * 13)), Color.Black);
                }

                if (obj is DebugMenu)
                {
                    DrawStrAt(e, $"WARNING: Cant view self", new Vector2f(10, e.Size.Y - 10 - FONT_SIZE), Color.Black);
                }
            }
        }
    }

    public const int FONT_SIZE = 24;

    public void DrawStrAt(RenderWindow e, string str, Vector2f pos, Color color)
    {
        Text text = new Text(str, Game.Instance.FontRepos.GetFont("arial"), FONT_SIZE);
        text.Position = pos;
        text.FillColor = color;
        e.Draw(text);
    }

    private void E_MouseLeft(object sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left)
            return;

        Camera2D cam = (Camera2D)Game.Instance.Level.ByClass<Camera2D>()[0];

        // store mouse pos to the form with the camera2d in mind
        Vector2f pos = new Vector2f(e.X, e.Y).ToCameraSpace(cam);

        // window it was called from
        RenderWindow window = (RenderWindow)sender;

        //Console.WriteLine(pos.X + " " + pos.Y);

        bool wasInMenu = false;

        // check what object is under the mouse
        foreach (RawObject obj in Game.Instance.Level.Children)
        {
            if (obj == null) continue;
            
            // calculate the bounding box of the object label
            FloatRect rect = new FloatRect(window.Size.X - 200, 0 + FONT_SIZE * Game.Instance.Level.Children.IndexOf(obj), 200, FONT_SIZE);


            // check if the mouse is inside the bounding box
            if (rect.Contains(pos.X, pos.Y))
            {
                wasInMenu = true;
                // if it is, expand the object
                CurrentExpanded = obj;
            }
        }

        if (!wasInMenu)
        {
            CurrentExpanded = null; // unselected so lets clear the onscreen debug info bout that object

            // lets create a new object at the mouse position thats rigidy
            RigidObject rigid = new RigidObject()
            {
                Position = new Vector2f(pos.X, pos.Y),
                Size = new Vector2f(10, 10),
                Color = new Color(0, 200, 0),
            };

            Game.Instance.Level.Children.Add(rigid);
        }
    }
}