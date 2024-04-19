using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState
{
    Vector2Int coords;
    bool isDead;


    public PlayerState(Player player)
    {
        coords = player.coords;
        isDead = player.isDead;
    }


    public void Restore(Player player)
    {
        player.ForceCoords(coords);
        player.UpdatePlayerCoords();
        player.isDead = isDead;
    }
}
