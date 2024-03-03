using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

public class Localization
{
    public string Language;
    public Dictionary<string, string> Texts = new Dictionary<string, string>();

    public void Init()
    {
        SetLanguage("es-es");
    }

    public void SetLanguage(string lang)
    {
        Language = lang;

        // load the language file
        string[] strings = File.ReadAllLines($"Data/Localization/{lang}.lang");
        foreach (string s in strings)
        {
            if (s.Length == 0 || s[0] == '#') continue; // allow blank lines and comments

            string[] parts = s.Split('=');
            Texts.Add(parts[0], parts[1]);
        }
    }

    public string GetText(string key)
    {
        if (Texts.ContainsKey(key))
            return Texts[key]; // return the localized string
        else return key; // return the placeholder for it (the key) which is in english
    }

    public string GetText(string key, string value)
    {
        if (Texts.ContainsKey(key))
            return Texts[key].Replace("{value}", value);
        else return key;
    }
}