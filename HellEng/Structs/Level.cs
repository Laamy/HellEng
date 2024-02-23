#region Includes

using SFML.Graphics;
using System.Collections.Generic;

#endregion

internal class Level
{
    public List<Object> children = new List<Object>();

    public void Draw(RenderWindow e)
    {
        // loop over the scene and draw u fuckign retard
        foreach (Object child in children)
        {
            child.Draw(e);
        }
    }

    public void Update(Game game)
    {
        // loop over the scene and update
        foreach (Object child in children)
        {
            child.Update(game);
        }
    }
}