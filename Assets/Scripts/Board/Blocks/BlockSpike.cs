using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpike : Block
{
    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection)
    {
        player.Die();
    }
}
