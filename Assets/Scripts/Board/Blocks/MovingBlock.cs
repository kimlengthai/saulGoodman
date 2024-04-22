using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : Block
{
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(player, playerDirection);
        player.coords += playerDirection;
    }


    protected override void OnPlayerBump(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerBump(player, playerDirection);
        Game.board.GetBlock(coords + playerDirection)?.PlayerBump(player, playerDirection);
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        return Game.board.CanPlayerMoveTo(player, coords + playerDirection, playerDirection);
    }
}
