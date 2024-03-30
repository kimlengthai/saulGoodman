using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpike : Block
{
    protected override void OnPlayerInteract(List<Player> players)
    {
        foreach (Player player in players)
        {
            player.Die();
        }
    }
}
