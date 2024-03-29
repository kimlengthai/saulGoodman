using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Block
{
    [SerializeField] bool isTransparent = false;

    [SerializeField] float animationSpeed;

    public override bool CanPlayerMoveInside()
    {
        return isTransparent;
    }


    protected override void UpdateSprite()
    {
        StartCoroutine(ChangeSpriteColor(new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            isTransparent ? 0.5f : 1f
        ), animationSpeed));
    }


    public void Unlock()
    {
        Debug.Log("Door unlocked!");
        isTransparent = true;
        UpdateSprite();
    }
}
