using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFlashing : Block
{
    [SerializeField] bool isTransparent;
    [SerializeField] float animationSpeed = 5f;


    void UpdateSprite()
    {
        StartCoroutine(ChangeStateAnimation());
    }


    public override bool CanPlayerMoveInside()
    {
        return isTransparent;
    }


    public override void OnTurnChange()
    {
        isTransparent = !isTransparent;
        UpdateSprite();
    }


    protected override IEnumerator ChangeStateAnimation()
    {
        float timeRatio = 0;

        while (timeRatio < 1)
        {
            timeRatio += Time.deltaTime * animationSpeed;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(isTransparent ? 1f : 0.5f, isTransparent ? 0.5f : 1f, timeRatio));
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, isTransparent ? 0.5f : 1f);
    }


    public override void UpdateBlock()
    {
        base.UpdateBlock();
        UpdateSprite();
    }
}
