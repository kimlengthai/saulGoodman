using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] Vector2Int startingCoords;
    [SerializeField] protected bool isTransparent;
    [SerializeField] protected bool isSolid;
    Vector2Int _coords;
    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            bool isPlacedInBoard = IsPlacedInBoard();

            if (isPlacedInBoard && coords == value)
                return;

            if (isPlacedInBoard)
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
        return !isSolid;
    }


    public virtual bool CanSeeThrough()
    {
        return isTransparent;
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


    protected virtual void OnPlayerInteract(List<Player> players, Vector2Int playerDirection) {}


    public void PlayerInteract(List<Player> players, Vector2Int playerDirection)
    {
        OnPlayerInteract(players, playerDirection);
        UpdateBlock();
    }


    protected virtual void OnPlayerEnter(List<Player> players, Vector2Int playerDirection) {}


    public void PlayerEnter(List<Player> players, Vector2Int playerDirection)
    {
        OnPlayerEnter(players, playerDirection);
        UpdateBlock();
    }


    protected virtual void OnPlayerBump(List<Player> players, Vector2Int playerDirection) {}


    public void PlayerBump(List<Player> players, Vector2Int playerDirection)
    {
        OnPlayerBump(players, playerDirection);
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
