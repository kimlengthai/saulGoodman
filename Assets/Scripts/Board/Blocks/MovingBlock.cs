using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : Block
{
    public AudioSource audioMovingBlock;
    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerEnter(player, playerDirection, animate);
        if (audioMovingBlock != null)
        {
            audioMovingBlock.Play();
        }
        player.QueueMove(playerDirection, animate);
    }


    protected override void OnPlayerBump(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerBump(player, playerDirection, animate);
        Game.board.GetBlock(coords + playerDirection)?.PlayerBump(player, playerDirection, animate);
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        return Game.board.CanPlayerMoveTo(player, coords + playerDirection, playerDirection);
    }
}

/*public void OnCollisionEnter3D(Collision2D collision)
{
    if (collision.gameObject.tag == "CollisionMovingBlockTag")
    {
        audioMovingBlock.Play();
    }
}*/