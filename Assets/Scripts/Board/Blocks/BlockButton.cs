using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Block
{
    [SerializeField] List<BlockDoor> doorsToUnlock;
    [SerializeField] float animationSpeed;


    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection)
    {
        UnlockDoors();
        StartCoroutine(ChangeSpriteColor(Color.green, animationSpeed));
    }


    void UnlockDoors()
    {
        foreach (BlockDoor door in doorsToUnlock)
        {
            door.Unlock();
        }
    }


    void LockDoors()
    {
        foreach (BlockDoor door in doorsToUnlock)
        {
            door.Lock();
        }
    }
}
