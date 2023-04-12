using System.Diagnostics;
using Godot;

public partial class anticheat : Node
{
    string[] suspiciousProcesses = { "cheat", "wemod" };
    string alertMessage;
    string alertTitle;
    public override void _Ready()
    {
        var lang = Json.ParseString(FileAccess.Open("res://assets/lang/en/warnings.json", FileAccess.ModeFlags.Read).GetAsText()).AsGodotDictionary();
        alertMessage = lang["cheatalert_message"].ToString();
        alertTitle = lang["cheatalert_title"].ToString();
    }
    public override void _Process(double delta)
    {
        foreach (Process p in Process.GetProcesses())
            foreach (string s in suspiciousProcesses)
            {
                if (p.ProcessName.Find(s) >= 0) //cheat gets detected
                {
                    GetTree().Paused = true;
                    OS.Kill(p.Id);
                    OS.Alert(alertMessage, alertTitle);
                    GetTree().Paused = false;
                }
            }
    }
}
