using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;


    [SerializeField] Vector2Int coordsInInspector;
    Vector2Int _coords;
    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            if (Game.board.GetBlock(value) != null || coords == value)
                return;

            if (IsPlacedInBoard())
                Game.board.MoveBlock(_coords, value);
            else 
                Game.board.SetBlock(this, value);
            
            _coords = value;

            transform.position = Game.board.GetBlockPosition(coords);
        }
    }

    public virtual void Start()
    {
        UpdateBlock();
    }


    bool IsPlacedInBoard()
    {
        return Game.board.GetBlock(coords) == this;
    }


    public virtual bool CanPlayerMoveInside()
    {
        return false;
    }


    public virtual void OnTurnChange() {}


    public virtual void UpdateBlock()
    {
        coords = _coords;
    }


    protected virtual IEnumerator ChangeStateAnimation()
    {
        yield return null;
    }


    void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;

            coords = coordsInInspector;
            UpdateBlock();
        };
    }
}
