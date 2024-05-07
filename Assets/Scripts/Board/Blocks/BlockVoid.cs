using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVoid : Block
{
    public override bool CanSeeThrough()
    {
        return true;
    }


    public override bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        return false;
    }


    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        base.OnValidate();

        if (Game.board != null)
            Game.board.OnValidate();
    }
    #endif
}