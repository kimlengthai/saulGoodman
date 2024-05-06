using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBump : Block
{
    [SerializeField] AudioSource audioBumpBlock;
    protected override void OnPlayerBump(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerBump(player, playerDirection, animate);

        if (audioBumpBlock != null)
        {
            audioBumpBlock.Play();
        }
        player.QueueMove(-playerDirection, animate);
    }
}
