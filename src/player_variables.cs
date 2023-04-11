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
            _playername = Regex.Replace(_playername, @"[\[\]<>\\\*\$\@\|\~\+§%/#\^°':;!?=\-`´卐卍࿕࿖࿗࿘ꖦ‍⃠\uD83C-\uDBFF\uDC00-\uDFFF]", "").Trim('"').StripEdges(); //todo swearword censoring with bbcode effect
            if (_playername.Length > 12)
                _playername = _playername.Substring(0, 12);
            return Regex.Replace(_playername, @"\s+", " ");
        }
        set { _playername = value; }
    }
}
