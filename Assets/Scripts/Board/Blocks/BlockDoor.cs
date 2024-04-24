using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Block
{
    [SerializeField] float animationSpeed;
    [SerializeField] bool startingOpen = false;

    bool? _open = null;
    public bool open
    {
        get => _open ?? startingOpen;
        set
        {
            if (_open == value)
                return;

            _open = value;
            isTransparent = open;
            UpdateSprite();
            Game.board.OnBoardChange();
        }
    }


    protected override void Init()
    {
        base.Init();
        open = startingOpen;
    }


    protected override void UpdateBlock()
    {
        open = _open ?? startingOpen;
        base.UpdateBlock();
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
