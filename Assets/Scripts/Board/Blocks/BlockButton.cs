using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Block
{
    [SerializeField] List<BlockDoor> doorsToUnlock;
    [SerializeField] float animationSpeed;
    bool isPressed = false;


    protected override void OnTurnChange()
    {
        isPressed = false;
    }


    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection)
    {
        isPressed = true;
    }


    protected override void OnTurnEnd()
    {
        if (isPressed)
            foreach (BlockDoor door in doorsToUnlock)
                door.open = !door.open;
    }


    protected override void UpdateSprite()
    {
        base.UpdateSprite();
        if (isPressed)
            StartCoroutine(PressAnimation());
    }


    IEnumerator PressAnimation()
    {
        yield return ChangeSpriteColor(Color.green, animationSpeed);
        yield return ChangeSpriteColor(defaultColor, animationSpeed);
    }


    public override Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = base.GetData();
        data["doorsToUnlock"] = doorsToUnlock.ConvertAll(door => door.coords);
        return data;
    }


    public override void SetData(Dictionary<string, object> data)
    {
        base.SetData(data);
        StartCoroutine(ResetDoorsToUnlockFromData(data));
    }


    IEnumerator ResetDoorsToUnlockFromData(Dictionary<string, object> data)
    {
        yield return new WaitForEndOfFrame();
        doorsToUnlock = ((List<Vector2Int>)data["doorsToUnlock"]).ConvertAll(coords => Game.board.GetBlock(coords) as BlockDoor);
    }
}
