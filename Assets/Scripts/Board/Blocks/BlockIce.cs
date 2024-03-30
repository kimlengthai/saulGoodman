using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIce : Block
{
    [SerializeField] private float slideSpeed = 5f;

    protected override void OnPlayerEnter(List<Player> players)
    {
        base.OnPlayerEnter(players);
        foreach (var player in players)
        {
            StartCoroutine(SlidePlayerAcrossIce(player));
        }
    }

    private IEnumerator SlidePlayerAcrossIce(Player player)
    {
        Vector2Int slideDirection = player.DirectionToVector2Int(player.lastMoveDirection); // Add lastMoveDirection to player to track their movement
        bool canSlide = true;

        while (canSlide)
        {
            Vector2Int nextCoords = player.coords + slideDirection;
            if (Game.board.CanPlayerMoveTo(nextCoords))
            {
                float timeToSlide = 1f / slideSpeed;
                Vector2 startPosition = player.transform.position;
                Vector2 endPosition = Game.board.GetBlockPosition(nextCoords);
                float slideTime = 0f;

                while (slideTime < 1f)
                {
                    slideTime += Time.deltaTime / timeToSlide;
                    player.transform.position = Vector2.Lerp(startPosition, endPosition, slideTime);
                    yield return null;
                }

                player.coords = nextCoords;
            }
            else
            {
                canSlide = false;
            }
        }
    }
}
