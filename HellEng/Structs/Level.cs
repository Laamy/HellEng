#region Includes

using SFML.Graphics;
using System;
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

    public RawObject ByTag(string tag)
    {
        foreach (RawObject child in Children)
        {
            if (child is SolidObject)
            {
                SolidObject obj = (SolidObject)child;

                if (obj.Tags.Contains(tag)) 
                    return obj;
            }

            if (child is LiquidObject)
            {
                LiquidObject obj = (LiquidObject)child;

                if (obj.Tags.Contains(tag))
                    return obj;
            }

            if (child is GroupObject)
            {
                GroupObject obj = (GroupObject)child;

                if (obj.Tags.Contains(tag))
                    return obj;
            }
        }

        return null;
    }

    public List<RawObject> ByClass<T>()
    {
        List<RawObject> list = new List<RawObject>();

        foreach (RawObject child in Children)
        {
            if (child is T)
            {
                list.Add(child);
            }
        }

        return list;
    }
}