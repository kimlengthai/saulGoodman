using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFlashing : Block
{
    [SerializeField] float animationSpeed = 5f;
    [SerializeField] Sprite[] sprites;
    public override void UpdateSprite()
    {
        base.UpdateSprite();
        spriteRenderer.sprite = sprites[isTransparent ? 0 : sprites.Length - 1];
    }


    public override IEnumerator Animation()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteRenderer.sprite = isTransparent ? sprites[sprites.Length - i - 1] : sprites[i];
            yield return new WaitForSeconds(1 / animationSpeed);
        }
    }


    protected override void OnTurnChange(bool animate)
    {
        isTransparent = !isTransparent;
        if (animate)
            StartCoroutine(PlayFX());
    }
}
