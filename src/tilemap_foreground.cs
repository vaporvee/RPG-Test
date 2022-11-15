using Godot;
using System;
using System.Linq;

public class tilemap_foreground : TileMap
{
    public override void _Process(float delta)
    {

    }

    public override void _Ready()
    {
        ReplaceStaticTiles(1);
    }

    public void ReplaceStaticTiles(int CellID)
    {
       Vector2[] allCells = GetUsedCellsById(CellID).OfType<Vector2>().ToArray(); 
        for(int i = 0; i < allCells.Length; i++)
        {
            GD.Print(allCells[i]);
        }
    }
}
