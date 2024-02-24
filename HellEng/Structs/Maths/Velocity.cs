using SFML.System;
using System.Collections.Concurrent;
using System.Collections.Generic;

internal class Velocity
{
    private ConcurrentDictionary<string, Vector2f> _vel = new ConcurrentDictionary<string, Vector2f>();

    public Vector2f this[string key]
    {
        get
        {
            if (!_vel.ContainsKey(key))
                _vel[key] = new Vector2f(0, 0);

            return _vel[key];
        }
        set => _vel[key] = value;
    }

    public Vector2f Main
    {
        get => this["_vel19253"];
        set => this["_vel19253"] = value;
    }

    public List<Vector2f> Values
    {
        get => new List<Vector2f>(_vel.Values);
    }

    public List<string> Keys
    {
        get => new List<string>(_vel.Keys);
    }
}