using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Block
{
    [SerializeField] BlockPortal destinationPortal;


    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(player, playerDirection);
        player.ForceCoords(destinationPortal.coords);

        Vector2 destinationPosition = Game.board.GetBlockPosition(destinationPortal.coords);
        player.AddAnimationToQueue(player.MovementAnimation(destinationPosition));
    }


    public override Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = base.GetData();
        data["destinationPortal"] = destinationPortal.coords;
        return data;
    }


    public override void SetData(Dictionary<string, object> data)
    {
        base.SetData(data);
        StartCoroutine(ResetDestinationPortalFromData(data));
    }


    IEnumerator ResetDestinationPortalFromData(Dictionary<string, object> data)
    {
        yield return new WaitForEndOfFrame();
        destinationPortal = Game.board.GetBlock((Vector2Int)data["destinationPortal"]) as BlockPortal;
    }
}
