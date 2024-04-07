using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIce : Block
{
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(player, playerDirection);
        SlidePlayerAcrossIce(player, playerDirection);
    }

    void SlidePlayerAcrossIce(Player player, Vector2Int playerDirection)
    {
        Vector2Int nextCoords = player.coords + playerDirection;

        while (Game.board.CanPlayerMoveTo(player, nextCoords))
        {
            player.coords = nextCoords;
            nextCoords = player.coords + playerDirection;
        }

        player.coords = nextCoords;
    }
}
