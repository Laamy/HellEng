#region Includes

using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

#endregion

internal class Object
{
    public virtual void Draw(RenderWindow e) { }
    public virtual void Update(Game game) { } // updates at a constant 60fps
}