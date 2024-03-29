using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Block
{
    public GameObject doorToUnlock;
    [SerializeField] bool isTransparent = true;

    public override bool CanPlayerMoveInside()
    {
        return isTransparent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.color = new Color(0,255,0);
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        doorToUnlock.GetComponent<Door>().Unlock();
    }
}
