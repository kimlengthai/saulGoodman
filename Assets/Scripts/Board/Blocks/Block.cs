using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Collider2D collision;
    [SerializeField] Vector2Int startingCoords;
    [SerializeField] bool _isTransparent = false;
    protected bool isTransparent
    {
        get { return !collision.enabled; }
        set { collision.enabled = !value; }
    }
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
        collision = GetComponent<Collider2D>();
        isTransparent = _isTransparent;
        coords = startingCoords;
        UpdateBlock();
    }


    bool IsPlacedInBoard()
    {
        return Game.board.GetBlock(coords) == this;
    }


    public virtual bool CanPlayerMoveInside(Player player)
    {
        if (player.isDead) return false;

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


    protected virtual void OnPlayerInteract(Player player, Vector2Int playerDirection) {}


    public void PlayerInteract(Player player, Vector2Int playerDirection)
    {
        OnPlayerInteract(player, playerDirection);
        UpdateBlock();
    }


    protected virtual void OnPlayerEnter(Player player, Vector2Int playerDirection) {}


    public void PlayerEnter(Player player, Vector2Int playerDirection)
    {
        OnPlayerEnter(player, playerDirection);
        PlayerInteract(player, playerDirection);
    }


    protected virtual void OnPlayerBump(Player player, Vector2Int playerDirection) {}


    public void PlayerBump(Player player, Vector2Int playerDirection)
    {
        OnPlayerBump(player, playerDirection);
        PlayerInteract(player, playerDirection);
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
