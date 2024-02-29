using SFML.Graphics;
using SFML.System;

internal class ClientInstance
{
    // fields (like matrices & class handlers)
    public Level Level = new Level();
    public FontRepository FontRepos = new FontRepository();
    public LocalPlayer Player = new LocalPlayer();
    public Camera2D Camera = new Camera2D();
    public DebugMenu DebugMenu = new DebugMenu();
}