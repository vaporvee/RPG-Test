using Godot;
using System;
using System.Text.RegularExpressions;

public partial class player_variables : Node
{
    private static string _playername = "Yannik";
    public static string PlayerName
    {
        get
        {
            _playername = Regex.Replace(_playername, "[^a-zA-Z0-9 ]+", "").StripEdges();
            return Regex.Replace(_playername, @"\s+", " ");
        }
        set { _playername = value; }
    }
}
