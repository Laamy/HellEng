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
    public Console Console = new Console();
    public Localization Localization = new Localization();
    //public GameSettings Settings = new GameSettings();
    //public GameTime Time = new GameTime();
    //public GameWindow Window = new GameWindow();
    //public GameInput Input = new GameInput();
    //public GameAudio Audio = new GameAudio();
    //public LoopbackSender LoopbackSender = new LoopbackSender();
    //public GameContent Content = new GameContent();
    //public GameRenderer Renderer = new GameRenderer();
    //public GameSave Save = new GameSave();
    //public GameMods Mods = new GameMods();
}