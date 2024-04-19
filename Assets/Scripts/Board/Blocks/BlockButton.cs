using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Block
{
    [SerializeField] List<BlockDoor> doorsToUnlock;
    [SerializeField] float animationSpeed;

    [SerializeField] bool startingPressed = false;

    bool _pressed;
    bool pressed
    {
        get { return _pressed; }
        set
        {
            _pressed = value;

            if (pressed)
                UnlockDoors();
            else
                LockDoors();
        }
    }


    protected override void Init()
    {
        base.Init();
        pressed = startingPressed;
    }


    protected override void OnPlayerInteract(Player player, Vector2Int playerDirection)
    {
        pressed = true;
    }


    protected override void UpdateSprite()
    {
        StartCoroutine(ChangeSpriteColor(pressed ? Color.green : defaultColor, animationSpeed));
    }


    void UnlockDoors()
    {
        foreach (BlockDoor door in doorsToUnlock)
            door.open = true;
    }


    void LockDoors()
    {
        foreach (BlockDoor door in doorsToUnlock)
            door.open = false;
    }


    public override Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = base.GetData();
        data["doorsToUnlock"] = doorsToUnlock.ConvertAll(door => door.coords);
        data["pressed"] = pressed;
        return data;
    }


    public override void SetData(Dictionary<string, object> data)
    {
        base.SetData(data);
        StartCoroutine(ResetDoorsToUnlockFromData(data));
        pressed = (bool)data["pressed"];
    }


    IEnumerator ResetDoorsToUnlockFromData(Dictionary<string, object> data)
    {
        yield return new WaitForEndOfFrame();
        doorsToUnlock = ((List<Vector2Int>)data["doorsToUnlock"]).ConvertAll(coords => Game.board.GetBlock(coords) as BlockDoor);
    }
}
