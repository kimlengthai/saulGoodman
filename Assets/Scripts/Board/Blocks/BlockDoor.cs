using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : Block
{
    [SerializeField] float animationSpeed;
    [SerializeField] bool startingOpen = false;
    [SerializeField] AudioSource audioDoorOpen;
    [SerializeField] AudioSource audioDoorClose;

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
        }
    }


    public override void Init()
    {
        base.Init();
        open = startingOpen;
    }


    public override void UpdateSprite()
    {
        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            open ? 0.5f : 1f
        );
    }


    public override IEnumerator Animation()
    {
        yield return null;
        StartCoroutine(ChangeSpriteColor(new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            open ? 0.5f : 1f
        ), animationSpeed));

        if (open)
            audioDoorOpen.Play();
        else
            audioDoorClose.Play();
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
