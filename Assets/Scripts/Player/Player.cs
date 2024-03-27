using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    up,
    down,
    left,
    right
}


public class Player : MonoBehaviour
{
    [SerializeField] Vector2Int _coords;
    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            if (!Game.board.CanPlayerMoveTo(value))
                return;
            
            _coords = value;
            transform.position = Game.board.GetBlockPosition(coords);
        }
    }


    public void Start()
    {
        coords = _coords;
    }


    public void OnTurnChange(Vector2Int direction)
    {
        coords += direction;
    }
}
