using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Block
{
    [SerializeField] float animationSpeed;

    [SerializeField] bool _open = false;
    public bool open
    {
        get => _open;
        set
        {
            _open = value;
            isSolid = !value;
            isTransparent = value;
            UpdateSprite();
        }
    }


    protected override void UpdateBlock()
    {
        open = _open;
        base.UpdateBlock();
    }


    protected override void UpdateSprite()
    {
        StartCoroutine(ChangeSpriteColor(new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            isSolid ? 1f : 0.5f
        ), animationSpeed));
    }


    public override Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = base.GetData();
        data.Add("open", open);
        return data;
    }


    public override void SetData(Dictionary<string, object> data)
    {
        base.SetData(data);
        open = (bool)data["open"];
    }
}
