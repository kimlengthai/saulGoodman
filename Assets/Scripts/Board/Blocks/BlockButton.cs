using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Block
{
    [SerializeField] List<BlockDoor> doorsToUnlock;
    [SerializeField] bool isTransparent = true;
    [SerializeField] float animationSpeed;

    public override bool CanPlayerMoveInside()
    {
        return isTransparent;
    }


    protected override void OnPlayerInteract()
    {
        UnlockDoors();
        StartCoroutine(ChangeSpriteColor(new Color(0f, 1f, 0f), animationSpeed));
    }


    void UnlockDoors()
    {
        foreach (BlockDoor door in doorsToUnlock)
        {
            door.Unlock();
        }
    }
}
