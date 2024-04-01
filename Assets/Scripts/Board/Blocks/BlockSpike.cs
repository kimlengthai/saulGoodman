using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpike : Block
{
    protected override void OnPlayerInteract(List<Player> players, Vector2Int playerDirection)
    {
        foreach (Player player in players)
        {
            player.Die();
        }
    }
}
