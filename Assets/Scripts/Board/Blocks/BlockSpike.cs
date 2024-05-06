using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpike : Block
{
    [SerializeField] AudioSource audioSpikeBlock;
    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection, bool animate)
    {
        if (audioSpikeBlock != null)
        {
            audioSpikeBlock.Play();
        }
        player.Die(animate);
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        return true;
    }
}
