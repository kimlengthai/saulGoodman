using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    public string blockName;
    [HideInInspector] public Color defaultColor;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Collider2D collision;
    [SerializeField] Vector2Int startingCoords;
    [SerializeField] bool _isTransparent;
    [SerializeField] GameObject blockInfoPrefab;
    GameObject blockInfo;
    protected AudioSource audioSource;
    protected bool isTransparent
    {
        get => _isTransparent;
        set
        {
            _isTransparent = value;
            gameObject.layer = value ? 0 : LayerMask.NameToLayer("Blocks");
        }
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
        audioSource = GetComponent<AudioSource>();
    }


    public virtual void Start()
    {
        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;
    }


    public virtual void Init()
    {
        collision.enabled = true;
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


    protected virtual void OnPlayersActionFinish(bool animate) {}


    public void PlayersActionFinish(bool animate)
    {
        OnPlayersActionFinish(animate);
    }


    protected virtual void OnTurnChange(bool animate) {}


    public void TurnChange(bool animate)
    {
        OnTurnChange(animate);
    }


    public virtual IEnumerator Animation()
    {
        yield return null;
        UpdateSprite();
    }


    public virtual void PlaySFX()
    {
        if (audioSource.clip == null) return;
        
        audioSource.Play();
    }


    public virtual IEnumerator PlayFX()
    {
        yield return null;

        PlaySFX();
        StartCoroutine(Animation());
    }


    public virtual void UpdateSprite() {}


    protected virtual void OnPlayerInteract(Player player, Vector2Int playerDirection, bool animate) {}


    public void PlayerInteract(Player player, Vector2Int playerDirection, bool animate)
    {
        OnPlayerInteract(player, playerDirection, animate);
    }


    protected virtual void OnPlayerEnter(Player player, Vector2Int playerDirection, bool animate) {}


    public void PlayerEnter(Player player, Vector2Int playerDirection, bool animate)
    {
        OnPlayerEnter(player, playerDirection, animate);
        PlayerInteract(player, playerDirection, animate);
    }


    protected virtual void OnPlayerBump(Player player, Vector2Int playerDirection, bool animate) {}


    public void PlayerBump(Player player, Vector2Int playerDirection, bool animate)
    {
        OnPlayerBump(player, playerDirection, animate);
        PlayerInteract(player, playerDirection, animate);
    }


    public IEnumerator ChangeSpriteColor(Color targetColor, float animationSpeed)
    {
        if (spriteRenderer == null)
            yield break;

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


    protected virtual void OnMouseEnter()
    {
        if (blockInfoPrefab == null) return;

        if (blockInfo == null)
            blockInfo = Instantiate(blockInfoPrefab, transform.position + new Vector3(0.45f, 0f, 0f), Quaternion.identity, Game.ui.transform);
        else
            blockInfo.SetActive(true);
    }


    protected virtual void OnMouseExit()
    {
        blockInfo?.SetActive(false);
    }


    public virtual Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["coords"] = coords;
        data["isTransparent"] = isTransparent;
        data["position"] = transform.localPosition;
        data["rotation"] = transform.localRotation;
        data["scale"] = transform.localScale;
        if (spriteRenderer != null)
            data["color"] = spriteRenderer.color;

        return data;
    }


    public virtual void SetData(Dictionary<string, object> data)
    {
        coords = (Vector2Int)data["coords"];
        isTransparent = (bool)data["isTransparent"];
        transform.localPosition = (Vector3)data["position"];
        transform.localRotation = (Quaternion)data["rotation"];
        transform.localScale = (Vector3)data["scale"];
        if (spriteRenderer != null)
            spriteRenderer.color = (Color)data["color"];
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
        };
    }
    #endif
}
