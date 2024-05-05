using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockIce : Block
{
    public AudioSource audioIceBlock;
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerEnter(player, playerDirection, animate);

        if (audioIceBlock != null)
        {
            audioIceBlock.Play();
        }

        if (player.isSliding)
            return;
        
        player.QueueAction(SlidePlayer(player, playerDirection, animate));
    }

    Action SlidePlayer(Player player, Vector2Int playerDirection, bool animate)
    {
        player.isSliding = true;

        return () =>
        {
            Block nextBlock = Game.board.GetBlock(player.coords + playerDirection);
            bool canMove = Game.board.CanPlayerMoveTo(player, player.coords + playerDirection, playerDirection);

            player.Move(playerDirection, animate);

            if (canMove)
                player.QueueAction(SlidePlayer(player, playerDirection, animate));
            else
                player.isSliding = false;
        };
    }
}
