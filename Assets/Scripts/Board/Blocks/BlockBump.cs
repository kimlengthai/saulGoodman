using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBump : Block
{
    protected override void OnPlayerBump(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerBump(player, playerDirection, animate);

        player.QueueMove(-playerDirection, animate);
        player.QueueAnimation(PlayFX());
    }
}
