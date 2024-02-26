using SFML.Graphics;
using SFML.System;

using System.Collections.Generic;

internal class GroupObject : RawObject
{
    // miniture level for inside of an actual level
    public List<SolidObject> Children = new List<SolidObject>();
    // ^ it'll be painful to maintain all objects so for now we'll just use SolidObject

    public void Add(Level level, SolidObject obj)
    {
        Children.Add(obj);
        level.Children.Add(obj);

        Position = Position; // update the position of the group & its children
    }

    public void Remove(Level level, SolidObject obj)
    {
        Children.Remove(obj);
        level.Children.Remove(obj);
    }

    public override void Update(Game game)
    {
        foreach (var child in Children)
            child.Update(game);
    }

    private Vector2f _position;

    public List<string> Tags;

    public Vector2f Position
    {
        get => _position;
        set
        {
            // set the position of the group
            _position = value;

            // offset each object in the group by the global position (position of the group)
            foreach (var child in Children)
                child.OffsetPosition = value;
        }
    }

    public override void Draw(RenderWindow e)
    {
        foreach (var child in Children)
        {
            child.Draw(e);
        }
    }
}