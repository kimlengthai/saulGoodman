using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFlashing : Block
{
    [SerializeField] bool isTransparent;


    public override void Start()
    {
        base.Start();

        UpdateSprite();
    }


    void UpdateSprite()
    {
        if (isTransparent)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        
        else
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }


    public override bool CanPlayerMoveInside()
    {
        return isTransparent;
    }


    public override void OnTurnChange()
    {
        print("BlockFlashing.OnTurnChange()");
        isTransparent = !isTransparent;
        UpdateSprite();
    }
}
