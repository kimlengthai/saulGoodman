using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPushable : Block
{
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(player, playerDirection);
        coords += playerDirection;
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        Vector2Int nextCoords = coords + playerDirection;

        if (!Game.board.IsInsideBoard(nextCoords))
            return false;
        
        return Game.board.GetBlock(nextCoords) == null;
    }
}
