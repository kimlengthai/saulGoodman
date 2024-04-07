using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : Block
{
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(player, playerDirection);
        MovePlayerOneSquare(player, playerDirection);
    }

    void MovePlayerOneSquare(Player player, Vector2Int playerDirection)
    {
        Vector2Int nextCoords = player.coords + playerDirection;

        if (Mathf.Abs(playerDirection.x) > Mathf.Abs(playerDirection.y))
        {
            Vector2Int slidingDirection = new Vector2Int(Mathf.RoundToInt(playerDirection.x), 0);
            nextCoords = player.coords + slidingDirection;
            if (Game.board.CanPlayerMoveTo(player, nextCoords))
            {
                player.coords = nextCoords;
            }
        }
        else
        {
            Vector2Int slidingDirection = new Vector2Int(0, Mathf.RoundToInt(playerDirection.y));
            nextCoords = player.coords + slidingDirection;
            if (Game.board.CanPlayerMoveTo(player, nextCoords))
            {
                player.coords = nextCoords;
            }
        }
    }
}
