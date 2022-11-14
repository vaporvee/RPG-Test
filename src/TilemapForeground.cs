using Godot;
using System;

public class TilemapForeground : TileMap
{
    [Export]
    bool ReplaceStaticTilesWithNodes = true;
    public override void _Process(float delta)
    {
     if(ReplaceStaticTilesWithNodes){
        ReplaceStaticTiles();
     }
    }

    public void ReplaceStaticTiles()
    {

    }
}
