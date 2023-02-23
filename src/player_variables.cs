using Godot;
using System;
using System.Text.RegularExpressions;

public partial class player_variables : Node
{
    private static string _playername = "Yannik";
    public static string PlayerName
    {
        get { return _playername; }
        set
        {
            _playername = Regex.Replace(value, @"\[[^]]+\]", "");
            _playername = Regex.Replace(_playername, @"<[^>]*>", "");
            if (PlayerName.Length > 12)
                _playername = PlayerName.Substring(0, 12);
        }
    }
}
