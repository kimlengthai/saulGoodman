using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Block
{
    [SerializeField] float animationSpeed;

    protected override void UpdateSprite()
    {
        StartCoroutine(ChangeSpriteColor(new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            isSolid ? 1f : 0.5f
        ), animationSpeed));
    }


    public void Unlock()
    {
        isSolid = false;
        isTransparent = true;
        UpdateSprite();
    }


    public void Lock()
    {
        isSolid = true;
        isTransparent = false;
        UpdateSprite();
    }
}
