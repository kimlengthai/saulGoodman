using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Block
{
    [SerializeField] float animationSpeed;

    [SerializeField] bool startingOpen = false;

    bool _open;
    public bool open
    {
        get => _open;
        set
        {
            _open = value;
            isSolid = !open;
            isTransparent = open;
            UpdateSprite();
        }
    }


    protected override void Init()
    {
        base.Init();
        open = startingOpen;
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
