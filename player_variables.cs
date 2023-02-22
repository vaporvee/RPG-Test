using Godot;
using System;
using System.Text.RegularExpressions;

public partial class player_variables : Node
{
    public static string PlayerName = "Yannik";

    public static void ClearPlayerName() //normal getter setter crashes for some reason
    {
        PlayerName = Regex.Replace(PlayerName, @"\[[^]]+\]", "");
        PlayerName = Regex.Replace(PlayerName, @"<[^>]*>", "");
        if (PlayerName.Length > 12)
            PlayerName = PlayerName.Substring(0, 12);
    }
}
