#region Includes

using SFML.Graphics;
using System.Collections.Generic;

#endregion

internal class Level
{
    public List<RawObject> Children = new List<RawObject>();

    public void Draw(RenderWindow e)
    {
        // loop over the scene and draw u fuckign retard
        foreach (RawObject child in Children)
        {
            child.Draw(e);
        }
    }

    public void Update(Game game)
    {
        // loop over the scene and update
        foreach (RawObject child in Children)
        {
            child.Update(game);
        }
    }
}