using System;
using System.Collections.Generic;

public struct ConsoleLog
{
    public string Data;
    public ConsoleColor Color;
}

public class Console
{
    public List<string> Logs = new List<string>(); // logs get stored in here

    public EventHandler<ConsoleLog> OnLog; // event handler for logging
    public EventHandler OnClear; // event handler for clearing the console

    private void append_logs(string message, ConsoleColor? colour = null)
    {
        Logs.Add(message); // add the message to the logs

        if (OnLog != null)
        {
            OnLog(this, new ConsoleLog() // new console log event packet
            {
                Data = message,
                Color = colour ?? ConsoleColor.Gray
            }); // invoke the event handler w/ console log packet
        }

        if (Logs.Count > 50)
            Logs.RemoveAt(0); // remove the first log if there are more than 50 logs
    }

    private string date_tostring() => DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ff"); // get the current date and time

    public void Clear()
    {
        Logs.Clear(); // clear the logs

        if (OnClear != null)
            OnClear(this, EventArgs.Empty); // invoke the event handler
    }

    [Obsolete("Use LogLocal instead")]
    public void Log(string message) => append_logs($"[Log, {date_tostring()}] {message}");

    [Obsolete("Use ErrorLocal instead")]
    public void Error(string message) => append_logs($"[Error, {date_tostring()}] {message}", ConsoleColor.Red);

    [Obsolete("Use WarnLocal instead")]
    public void Warn(string message) => append_logs($"[Warn, {date_tostring()}] {message}", ConsoleColor.Yellow);

    [Obsolete("Use InfoLocal instead")]
    public void Info(string message) => append_logs($"[Info, {date_tostring()}] {message}", ConsoleColor.Cyan);

    public void LogL(string message) => Log(Game.Instance.Localization.GetText(message));
    public void ErrorLocal(string message) => Error(Game.Instance.Localization.GetText(message));
    public void WarnLocal(string message) => Warn(Game.Instance.Localization.GetText(message));
    public void InfoLocal(string message) => Info(Game.Instance.Localization.GetText(message));
}