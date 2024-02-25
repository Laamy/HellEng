using SFML.Graphics;
using SFML.System;
using System;

internal class RigidObject : SolidObject
{
    public Velocity Velocity { get; } = new Velocity(); // body velocity

    public float Gravity = 1.3f; // gravity force applied to the object
    public float AirFriction = 0.1f; // air drag force applied to the object

    public bool Colliding = false; // is the object colliding with anything?
    public bool Grounded = false; // is the object grounded?
    public bool InWater = false; // is the object in water?

    public override void Update(Game game)
    {
        Velocity.Main = new Vector2f(Velocity.Main.X, Velocity.Main.Y + Gravity); // apply gravity to main velocity

        bool hasCollided = false; // if the object has collided with anything
        bool inLiquid = false; // if the object is in water

        // check if the player is colliding with anything (excluding water as we need to seperately check before we apply friction)
        foreach (RawObject obj in Game.Instance.Level.Children)
        {
            if (obj is LiquidObject)
            {
                if (obj == null) continue; // skip if null

                LiquidObject liquid = (LiquidObject)obj;

                if (Bounds.ResolveCollision(liquid.Bounds, out bool xAxis, false))
                {
                    // we're inside of the liquid but its still possible that we're inside a clipped area
                    // if we are we shouldnt act as we're in liquid
                    bool inClipping = false;

                    foreach (var clip in liquid.ClippingAreas)
                    {
                        FloatRect tempBounds = clip.Value;

                        // offset the tempBounds by the liquid position
                        tempBounds.Left += liquid.Position.X;
                        tempBounds.Top += liquid.Position.Y;

                        if (Bounds.IsInside(tempBounds))
                            inClipping = true;
                    }

                    if (!inClipping)
                    {
                        // not in a clipping area so act as we're in liquid
                        inLiquid = true;
                        Grounded = true;
                        hasCollided = true;

                        foreach (string key in Velocity.Keys)
                            Velocity[key] /= 1 + liquid.Friction;
                    }
                    else
                    {
                        inLiquid = false;
                        Grounded = false;
                        hasCollided = false;
                    }
                }
            }
        }

        foreach (string key in Velocity.Keys) // apply air drag to velocities
            Velocity[key] /= 1 + AirFriction;

        foreach (Vector2f _vel in Velocity.Values) // apply velocities to position
            Position += _vel;

        foreach (RawObject obj in Game.Instance.Level.Children)
        {
            if (obj is SolidObject)
            {
                if (obj == null) continue; // skip if null

                SolidObject solid = (SolidObject)obj;

                if (solid == this) continue; // skip if it's the player

                if (Bounds.ResolveCollision(solid.Bounds, out bool xAxis))
                {
                    Vector2f m_Velocity = Velocity.Main;

                    if (xAxis)
                        m_Velocity.X = 0;
                    else
                    {
                        Grounded = true; // for now this counts as up and down grounded (you can jump on the roof)
                        m_Velocity.Y = 0;
                    }

                    Velocity.Main = m_Velocity;

                    hasCollided = true;
                }
            }
        }

        InWater = inLiquid; // set the in water state
        Colliding = hasCollided; // set the colliding state
        Grounded = hasCollided ? Grounded : hasCollided; // set the grounded state (if not colliding we set to false to avoid air jumps)
    }
}