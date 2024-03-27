using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static float width;
    public static float height;

    [SerializeField] protected SpriteRenderer spriteRenderer;


    [SerializeField] Vector2Int _coords;
    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            if (Game.board.GetBlock(value) != null)
                return;

            _coords = value;
            Game.board.SetBlock(this, coords);
        }
    }

    public virtual void Start()
    {
        coords = _coords;
    }


    public virtual bool CanPlayerMoveInside()
    {
        return false;
    }


    public virtual void OnTurnChange() { print(this); }
}
