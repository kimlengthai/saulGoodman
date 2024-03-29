using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [SerializeField] Vector2Int startingCoords;
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
        coords = startingCoords;
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


    protected virtual void OnTurnChange() {}


    public void TurnChange()
    {
        OnTurnChange();
        UpdateBlock();
    }


    protected virtual void UpdateBlock()
    {
        coords = _coords;
        UpdateSprite();
    }


    protected virtual void UpdateSprite() {}


    protected virtual void OnPlayerInteract() {}


    protected virtual void OnPlayerEnter() {}


    public void PlayerEnter()
    {
        OnPlayerEnter();
        OnPlayerInteract();
        UpdateBlock();
    }


    protected virtual void OnPlayerBump() {}


    public void PlayerBump()
    {
        OnPlayerBump();
        OnPlayerInteract();
        UpdateBlock();
    }


    protected IEnumerator ChangeSpriteColor(Color targetColor, float animationSpeed)
    {
        float timeRatio = 0;
        Color initialColor = spriteRenderer.color;

        while (timeRatio < 1)
        {
            timeRatio += Time.deltaTime * animationSpeed;
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, timeRatio);
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }


    void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;

            coords = startingCoords;
            UpdateBlock();
        };
    }
}
