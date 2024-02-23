using SFML.Graphics;
using SFML.System;

internal class SolidText : Object
{
    public Vector2f Position { get; set; }
    public uint Size { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public string Text { get; set; }
    public Font Font { get; set; }

    public override void Draw(RenderWindow e)
    {
        Text text = new Text(Text, Font, Size);
        text.Position = Position;
        text.FillColor = Color;
        text.Rotation = Rotation;

        e.Draw(text);
    }
}