using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    public string blockName;
    [HideInInspector] public Color defaultColor;
    [HideInInspector] public bool shouldInit = true;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Collider2D collision;
    [SerializeField] Vector2Int startingCoords;
    [SerializeField] bool _isTransparent;
    protected bool isTransparent
    {
        get { return !collision.enabled; }
        set { collision.enabled = !value; }
    }

    public bool hasInit
    {
        get => IsPlacedInBoard();
    }

    Vector2Int _coords = new Vector2Int(-1, -1);
    public virtual Vector2Int coords
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


    public virtual void Awake()
    {
        collision = GetComponent<Collider2D>();
    }


    public virtual void Start()
    {
        if (shouldInit)
            Init();

        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;
        
        UpdateBlock();
    }


    protected virtual void Init()
    {
        isTransparent = _isTransparent;
        coords = startingCoords;
    }


    bool IsPlacedInBoard()
    {
        return Game.board.GetBlock(coords) == this;
    }


    public virtual bool CanPlayerMoveInside(Player player, Vector2Int playerDirection)
    {
        if (player.isDead) return false;

        return isTransparent;
    }


    public virtual bool CanSeeThrough()
    {
        return isTransparent;
    }


    protected virtual void OnBoardChange() {}


    public void BoardChange()
    {
        if (!hasInit) return;

        OnBoardChange();
        UpdateBlock();
    }


    protected virtual void OnTurnChange() {}


    public void TurnChange()
    {
        OnTurnChange();
        UpdateBlock();
    }


    protected virtual void OnTurnEnd() {}

    public void TurnEnd()
    {
        OnTurnEnd();
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


    public IEnumerator ChangeSpriteColor(Color targetColor, float animationSpeed)
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


    public virtual Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["coords"] = coords;
        data["isTransparent"] = isTransparent;
        data["position"] = transform.localPosition;
        data["rotation"] = transform.localRotation;
        data["scale"] = transform.localScale;

        return data;
    }


    public virtual void SetData(Dictionary<string, object> data)
    {
        coords = (Vector2Int)data["coords"];
        isTransparent = (bool)data["isTransparent"];
        transform.localPosition = (Vector3)data["position"];
        transform.localRotation = (Quaternion)data["rotation"];
        transform.localScale = (Vector3)data["scale"];
    }


    #if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;

            Init();
            Game.board.OnBoardChange();
        };
    }
    #endif
}
