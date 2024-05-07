using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpike : Block
{
    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection, bool animate)
    {
        player.Die(animate);
        if (animate)
            player.QueueAnimation(PlayFX());
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        return true;
    }
}
