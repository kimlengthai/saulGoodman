using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRotation : Block
{
    [SerializeField] bool invertRotation = false;

    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(player, playerDirection);
        player.coords += RotateDirection(playerDirection);
    }


    protected override void OnPlayerBump(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerBump(player, playerDirection);

        Vector2Int newDirection = RotateDirection(playerDirection);
        Game.board.GetBlock(coords + newDirection)?.PlayerBump(player, newDirection);
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        Vector2Int newDirection = RotateDirection(playerDirection);
        return Game.board.CanPlayerMoveTo(player, coords + newDirection, newDirection);
    }


    Vector2Int RotateDirection(Vector2Int direction)
    {
        if (invertRotation)
            return new Vector2Int(direction.y, direction.x);
        return new Vector2Int(-direction.y, -direction.x);
    }


    protected override void UpdateSprite()
    {
        base.UpdateSprite();

        if (invertRotation)
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 90);
    }
}
