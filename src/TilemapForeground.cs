using Godot;
using System;

public class TilemapForeground : TileMap
{
    [Export]
    bool ReplaceStaticTilesWithNodes = true;
    public override void _Process(float delta)
    {

    }

    public override void _Ready()
    {
        if (ReplaceStaticTilesWithNodes)
        {
            ReplaceStaticTiles(1);
        }
    }

    public void ReplaceStaticTiles(int CellID)
    {
        Godot.Collections.Array allcells = GetUsedCellsById(CellID); //why isnt it just a normal array wtf
        for(int i = 0; i < allcells.Count; i++)
        {
            GD.Print(allcells[i]); //needs a bit more stuff
        }
    }
}
