using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainPlayer : Player
{
    void OnUp()
    {
        Game.OnTurnChange(Vector2Int.up);
    }


    void OnDown()
    {
        Game.OnTurnChange(Vector2Int.down);
    }


    void OnLeft()
    {
        Game.OnTurnChange(Vector2Int.left);
    }


    void OnRight()
    {
        Game.OnTurnChange(Vector2Int.right);
    }
}
