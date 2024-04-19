using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBump : Block
{
    protected override void OnPlayerBump(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerBump(player, playerDirection);
        player.coords -= playerDirection; 
    }
}
