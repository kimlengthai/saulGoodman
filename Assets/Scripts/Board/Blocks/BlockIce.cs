using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockIce : Block
{
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerEnter(player, playerDirection, animate);
        player.QueueAction(SlidePlayerAcrossIce(player, playerDirection, animate));
    }

    Action SlidePlayerAcrossIce(Player player, Vector2Int playerDirection, bool animate)
    {
        return () =>
        {
            Block nextBlock = Game.board.GetBlock(player.coords + playerDirection);

            player.Move(playerDirection, animate);

            if (Game.board.CanPlayerMoveTo(player, player.coords + playerDirection, playerDirection) && nextBlock is not BlockIce)
                player.QueueAction(SlidePlayerAcrossIce(player, playerDirection, animate));
        };
    }
}
