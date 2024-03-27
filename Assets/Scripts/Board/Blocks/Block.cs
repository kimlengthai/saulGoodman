using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
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
            transform.position = Game.board.GetBlockPosition(coords);
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

            UpdateBlock();
        };
    }
}
