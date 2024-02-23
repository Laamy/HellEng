using SFML.Graphics;
using SFML.System;

internal class ClientInstance
{
    // fields (like matrices & class handlers)
    public Level Level = new Level();
    public FontRepository FontRepos = new FontRepository();
    public LocalPlayer Player = new LocalPlayer();
    public SolidText DebugText = new SolidText()
    {
        Position = new Vector2f(10, 10),
        Size = 16,
        Color = Color.Black,
    };

    // methods
    public void ClearDebugText()
        => DebugText.Text = string.Empty;

    public void SetDebugText(string[] text)
    {
        if (DebugText.Font == null)
            DebugText.Font = FontRepos.GetFont("arial");

        ClearDebugText(); // clear the text

        // add each line of text
        foreach (string line in text)
            DebugText.Text += line + "\n";
    }
}