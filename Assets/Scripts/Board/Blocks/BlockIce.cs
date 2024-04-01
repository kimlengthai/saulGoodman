using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIce : Block
{
    [SerializeField] private float slideSpeed = 5f;

    protected override void OnPlayerEnter(List<Player> players, Vector2Int playerDirection)
    {
        base.OnPlayerEnter(players, playerDirection);
        foreach (var player in players)
        {
            StartCoroutine(SlidePlayerAcrossIce(player, playerDirection));
        }
    }

    private IEnumerator SlidePlayerAcrossIce(Player player, Vector2Int playerDirection)
    {
        float slideTime = 0f;
        while (slideTime < 1f)
        {
            slideTime += Time.deltaTime * player.speed;
            yield return null;
        }
        
        Vector2Int nextCoords = player.coords + playerDirection;

        while (Game.board.CanPlayerMoveTo(nextCoords))
        {
            slideTime = 0f;
            player.coords = nextCoords;

            while (slideTime < 1f)
            {
                slideTime += Time.deltaTime * slideSpeed;
                yield return null;
            }

            nextCoords = player.coords + playerDirection;
        }
    }
}
