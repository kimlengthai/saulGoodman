using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Block
{
    [SerializeField] bool isTransparent = false;

    public override bool CanPlayerMoveInside()
    {
        return isTransparent;
    }

    public void Unlock()
    {
        Debug.Log("Door unlocked!");
        isTransparent = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, isTransparent ? 0.5f : 1f);
    }
}
