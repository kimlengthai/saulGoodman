using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Block
{
    [SerializeField] List<BlockDoor> doorsToUnlock;
    [SerializeField] float animationSpeed;
    [SerializeField] Sprite[] sprites;
    bool isPressed = false;
    bool leftState = true;
    bool shouldUpdateSprite = false;


    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection, bool animate)
    {
        isPressed = true;
        if (animate)
            shouldUpdateSprite = true;
    }


    protected override void OnPlayersActionFinish(bool animate)
    {
        if (isPressed)
            foreach (BlockDoor door in doorsToUnlock)
                door.open = !door.open;
        
        isPressed = false;
    }


    public override void UpdateSprite()
    {
        base.UpdateSprite();
        if (shouldUpdateSprite)
            StartCoroutine(PressAnimation());
        
        shouldUpdateSprite = false;
    }


    IEnumerator PressAnimation()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * animationSpeed;

            int state = Mathf.FloorToInt(time * sprites.Length);
            if (state == sprites.Length) state--;
            spriteRenderer.sprite = sprites[leftState ? state : sprites.Length - 1 - state];
            yield return null;
        }

        leftState = !leftState;
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
