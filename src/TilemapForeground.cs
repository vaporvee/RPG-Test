using Godot;
using System;
using System.Linq;

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
        Vector2[] allcells = GetUsedCellsById(CellID).OfType<Vector2>().ToArray();
        for(int i = 0; i < allcells.Length; i++)
        {
            GD.Print(allcells[i]); //now you can get the coordination to spawn a replacement node and get x and y seperately for removing this tile
        }
    }
}
