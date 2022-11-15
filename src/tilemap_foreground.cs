using Godot;
using System;
using System.Linq;

public class tilemap_foreground : TileMap
{
    public override void _Process(float delta)
    {
        ReplaceStaticTiles(1, "debug_tile_one");
    }

    public void ReplaceStaticTiles(int CellID, string sceneName)
    {
       Vector2[] allCells = GetUsedCellsById(CellID).OfType<Vector2>().ToArray(); 
        for(int i = 0; i < allCells.Length; i++)
        {
            GD.Print(allCells[i]);
            SetCell((int)allCells[i].x, (int)allCells[i].y, -1);
            var scene = GD.Load<PackedScene>("res://scenes/interactable_tiles/" + sceneName + ".tscn");
            var instance = scene.Instance();
            AddChild(instance);
            var node = GetNode<Node2D>(instance.GetPath());
            node.Position = allCells[i] * CellSize; //node has to be Node2D and can't be centered
            GD.Print(node);
        }
    }
}
