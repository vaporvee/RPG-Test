using Godot;
using System;

public class tilemap_foreground : TileMap
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
       //Vector2[] AllCells = GetUsedCellsById(CellID); //why isnt it just a normal array wtf
       // for(int i = 0; i == AllCells.Length; i++)
       // {
       //     GD.Print(AllCells[i]);
       // }
    }
}
