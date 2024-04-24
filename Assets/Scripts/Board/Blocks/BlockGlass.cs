using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGlass : Block
{
    public override Vector2Int coords
    {
        get { return base.coords; }
        set
        {
            Vector2Int previousCoords = coords;
            base.coords = value;
            
            string previousCoordsString = $"({previousCoords.x}, {previousCoords.y})";
            if (Game.board.IsInsideBoard(previousCoords) && Game.board.background.transform.Find(previousCoordsString) == null)
                Game.board.AddBackgroundBlock(previousCoords);
            
            Transform backgroundBlock = Game.board.background.transform.Find($"({coords.x}, {coords.y})");
            if (backgroundBlock != null)
                DestroyImmediate(backgroundBlock.gameObject);
        }
    }


    public override bool CanSeeThrough()
    {
        return true;
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        return false;
    }
}