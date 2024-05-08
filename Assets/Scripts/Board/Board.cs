using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[ExecuteInEditMode]
public class Board : MonoBehaviour
{
    [HideInInspector] public string levelName;
    public int width;
    public int height;

    List<BoardState> boardStates = new List<BoardState>();

    Vector2 offset;

    [SerializeField] float blockWidth;
    [SerializeField] float blockHeight;

    [SerializeField] GameObject emptyBlockPrefab;
    public GameObject background;
    [SerializeField] GameObject backgroundPrefab;
    public GameObject blocksParent;

    Block[,] blocks;


    bool _hasInit = false;
    public bool hasInit
    {
        get
        {
            if (!_hasInit)
                return false;
            
            foreach (Block block in blocks)
                if (block != null && !block.hasInit)
                    return false;
            
            return true;
        }
    }


    public void Awake()
    {
        blocks = new Block[width, height];
        ResetOffset();
    }


    public void Start()
    {
        levelName = SceneManager.GetActiveScene().name;
        _hasInit = true;
    }


    void InitBlocks()
    {
        foreach (Transform blockTransform in blocksParent.transform)
        {
            Block block = blockTransform.GetComponent<Block>();
            if (block == null)
                continue;
            
            block.Init();
        }
    }


    public void Init()
    {
        InitBlocks();
        
        foreach (Player player in Game.players)
            player.Init();
    }


    public void ResetBoardState()
    {
        boardStates.Clear();
        SaveBoardState();
    }


    void ResetOffset()
    {
        offset = new Vector2(
            transform.position.x - width * blockWidth / 2 + blockWidth / 2,
            transform.position.y - height * blockHeight / 2 + blockHeight / 2
        );
    }


    public bool IsInsideBoard(Vector2Int coords)
    {
        return (coords.x >= 0 && coords.x < width && coords.y >= 0 && coords.y < height);
    }


    public Block GetBlock(Vector2Int coords)
    {
        if (IsInsideBoard(coords))
            return blocks[coords.x, coords.y];
        return null;
    }


    public void SetBlock(Block block, Vector2Int coords)
    {
        if (!IsInsideBoard(coords))
        {
            Debug.LogError("Trying to set a block outside the board");
            return;
        }
        if (blocks[coords.x, coords.y] != null)
        {
            Debug.LogError("Trying to set a block where there is already a block");
            return;
        }

        blocks[coords.x, coords.y] = block;
    }


    public void RemoveBlock(Vector2Int coords, bool destroy = true)
    {
        if (!IsInsideBoard(coords))
        {
            Debug.LogError("Trying to remove a block outside the board");
            return;
        }

        if (blocks[coords.x, coords.y] == null)
            return;

        if (destroy)
            Destroy(blocks[coords.x, coords.y].gameObject);
        
        blocks[coords.x, coords.y] = null;
    }


    public void MoveBlock(Vector2Int from, Vector2Int to)
    {
        if (!IsInsideBoard(from) || !IsInsideBoard(to))
        {
            Debug.LogError("Trying to move a block outside the board");
            return;
        }

        if (blocks[to.x, to.y] != null)
        {
            Debug.LogError("Trying to move a block where there is already a block");
            return;
        }

        if (blocks[from.x, from.y] == null)
        {
            Debug.LogError("Trying to move a block that doesn't exist");
            return;
        }

        blocks[to.x, to.y] = blocks[from.x, from.y];
        blocks[from.x, from.y] = null;
    }


    public void Clear()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                RemoveBlock(new Vector2Int(x, y));
    }


    public Vector2 GetBlockPosition(Vector2Int coords)
    {
        return new Vector2(coords.x * blockWidth, coords.y * blockHeight) + offset;
    }


    public bool CanPlayerMoveTo(Player player, Vector2Int coords, Vector2Int playerDirection)
    {
        if (!IsInsideBoard(coords) || player.isDead)
            return false;
        
        Block block = GetBlock(coords);

        if (block == null)
            return true;
        
        return block.CanPlayerMoveInside(player, playerDirection);
    }


    public void OnTurnChange(bool animate)
    {
        foreach (Block block in blocks)
            if (block != null)
                block.TurnChange(animate);
    }


    public void OnPlayersActionFinish(bool animate)
    {
        foreach (Block block in blocks)
            if (block != null && block is not BlockLaser)
                block.PlayersActionFinish(animate);
        
        foreach (Block block in blocks)
            if (block != null && block is BlockLaser)
                block.PlayersActionFinish(animate);
    }


    void AddBackgroundBlock(Vector2Int coords)
    {
        GameObject newBlock = Instantiate(emptyBlockPrefab, background.transform);
        newBlock.transform.position = GetBlockPosition(coords);
        newBlock.transform.localScale = new Vector3(blockWidth, blockHeight, 1);
        newBlock.name = $"({coords.x}, {coords.y})";
    }


    public void UpdateBoard()
    {
        if (this == null) return;

        Init();

        foreach (Transform blockTransform in blocksParent.transform)
            blockTransform.localScale = new Vector3(blockWidth, blockHeight, 1);

        foreach (Player player in Game.players)
            player.transform.localScale = new Vector3(blockWidth, blockHeight, 1);
        
        if (background == null)
            background = GameObject.Find("Background");
        DestroyImmediate(background);

        background = Instantiate(backgroundPrefab);
        background.name = "Background";
        
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (blocks[x, y] is not BlockVoid)
                    AddBackgroundBlock(new Vector2Int(x, y));

        foreach (Block block in blocks)
            if (block != null)
                block.UpdateSprite();
    }


    public void UndoLastMove()
    {
        if (boardStates.Count <= 1)
            return;

        boardStates.RemoveAt(boardStates.Count - 1);
        RestoreLastBoardState();

        Game.turn--;
    }


    public void RestoreLastBoardState()
    {
        boardStates[boardStates.Count - 1].Restore(this);

        foreach (Block block in blocks)
            if (block != null)
                block.UpdateSprite();
    }


    public void SaveBoardState()
    {
        boardStates.Add(new BoardState(this));
    }


    #if UNITY_EDITOR
    public void OnValidate()
    {
        Awake();

        UnityEditor.EditorApplication.delayCall += UpdateBoard;
    }
    #endif
}
