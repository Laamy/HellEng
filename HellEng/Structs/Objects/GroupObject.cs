using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

internal class GroupObject : SolidObject
{
    // miniture level for inside of an actual level
    public List<SolidObject> Children = new List<SolidObject>();
    // ^ it'll be painful to maintain all objects so for now we'll just use SolidObject

    public override void Update(Game game)
    {
        foreach (var child in Children)
            child.Update(game);
    }

    public override void Draw(RenderWindow e)
    {
        foreach (var child in Children)
        {
            Vector2f pos = child.Position; // save the position

            child.Position += Position; // add the group's position to the child's position when rendering
            child.Draw(e);

            child.Position = pos; // restore the position
        }
    }
}