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
            isTransparent ? 0.5f : 1f
        ), animationSpeed));
    }


    public void Unlock()
    {
        isSolid = false;
        UpdateSprite();
    }


    public void Lock()
    {
        isSolid = true;
        UpdateSprite();
    }
}
