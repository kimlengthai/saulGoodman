using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPortal : Block
{
    [SerializeField] BlockPortal destinationPortal;
    [SerializeField] AudioSource audioPortalBlock;

    protected override void OnPlayerEnter(Player player, Vector2Int playerDirection, bool animate)
    {
        base.OnPlayerEnter(player, playerDirection, animate);

        player.ForceCoords(destinationPortal.coords);

        if (animate)
        {
            Vector2 destinationPositionCoords = Game.board.GetBlockPosition(destinationPortal.coords);
            player.QueueAnimation(PlayFX());
            player.QueueAnimation(player.MovementAnimation(destinationPositionCoords));
        }
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
