using System;
using System.IO;

internal class Debug
{
    public static bool IsConsoleOpen()
    {
        try
        {
            return Console.WindowHeight > 0;
        }
        catch (IOException ex)
        {
            return false;
        }
    }

    public static void Log(string message)
    {
        if (IsConsoleOpen())
            Console.WriteLine(message);
        else
            System.Diagnostics.Debug.WriteLine(message);
    }
}