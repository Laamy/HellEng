using SFML.System;

internal class RigidObject : SolidObject
{
    public Velocity Velocity { get; } = new Velocity(); // body velocity

    public float Gravity = 0.8f; // gravity force applied to the object
    public float AirDrag = 0.1f; // air drag force applied to the object

    public bool Colliding = false; // is the object colliding with anything?
    public bool Grounded = false; // is the object grounded?

    public override void Update(Game game)
    {
        // apply gravity to main velocity
        Velocity.Main = new Vector2f(Velocity.Main.X, Velocity.Main.Y + Gravity);

        //apply air drag to velocities
        foreach (string key in Velocity.Keys)
            Velocity[key] /= 1 + AirDrag;

        // apply velocities to position
        foreach (Vector2f _vel in Velocity.Values)
            Position += _vel;

        // check if the player is colliding with anything
        foreach (RawObject obj in Game.Instance.Level.children)
        {
            if (!(obj is SolidObject)) continue; // skip if not a solid object
            if (obj == null) continue; // skip if null

            SolidObject solid = (SolidObject)obj;

            if (solid == this) continue; // skip if it's the player

            if (Bounds.ResolveCollision(solid.Bounds, out bool xAxis))
            {
                Colliding = true;

                Vector2f m_Velocity = Velocity.Main;

                if (xAxis)
                    m_Velocity.X = 0;
                else
                {
                    Grounded = true; // for now this counts as up and down grounded (you can jump on the roof)
                    m_Velocity.Y = 0;
                }

                Velocity.Main = m_Velocity;
            }
            else
            {
                Colliding = false;
                Grounded = false;
            }
        }
    }
}