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
    GameObject background;
    public GameObject blocksParent;

    Block[,] blocks;


    bool _hasInit = false;
    public bool hasInit
    {
        get
        {
            if (!_hasInit)
                return false;
            
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (blocks[x, y] != null && !blocks[x, y].hasInit)
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


    public void OnBoardChange()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (blocks[x, y] != null)
                    blocks[x, y].BoardChange();
    }


    public Block GetBlock(Vector2Int coords)
    {
        if (IsInsideBoard(coords))
            return blocks[coords.x, coords.y];
        return null;
    }


    public void SetBlock(Block block, Vector2Int coords, bool boardChange = true)
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

        if (boardChange)
            OnBoardChange();
    }


    public void RemoveBlock(Vector2Int coords, bool boardChange = true)
    {
        if (!IsInsideBoard(coords))
        {
            Debug.LogError("Trying to remove a block outside the board");
            return;
        }

        if (blocks[coords.x, coords.y] == null)
            return;

        Destroy(blocks[coords.x, coords.y].gameObject);
        blocks[coords.x, coords.y] = null;

        if (boardChange)
            OnBoardChange();
    }


    public void MoveBlock(Vector2Int from, Vector2Int to, bool boardChange = true)
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

        if (boardChange)
            OnBoardChange();
    }


    public void Clear(bool boardChange = true)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                RemoveBlock(new Vector2Int(x, y), false);
        
        if (boardChange)
            OnBoardChange();
    }


    public Vector2 GetBlockPosition(Vector2Int coords)
    {
        return new Vector2(coords.x * blockWidth, coords.y * blockHeight) + offset;
    }


    public bool CanPlayerMoveTo(Player player, Vector2Int coords)
    {
        if (!IsInsideBoard(coords) || player.isDead)
            return false;
        
        Block block = GetBlock(coords);

        if (block == null)
            return true;
        
        return block.CanPlayerMoveInside(player);
    }


    public void OnTurnChange()
    {
        foreach (Block block in blocks)
        {
            if (block != null)
                block.TurnChange();
        }
    }


    void UpdateBoard()
    {
        if (this == null) return;

        if (background == null)
            background = GameObject.Find("Background");
        DestroyImmediate(background);

        background = new GameObject("Background");
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newBlock = Instantiate(emptyBlockPrefab, background.transform);
                newBlock.transform.position = GetBlockPosition(new Vector2Int(x, y));
                newBlock.transform.localScale = new Vector3(blockWidth, blockHeight, 1);
                newBlock.name = $"({x}, {y})";
            }
        }

        foreach (Transform blockTransform in blocksParent.transform)
        {
            Block block = blockTransform.GetComponent<Block>();

            block.coords = block.coords;
            blockTransform.localScale = new Vector3(blockWidth, blockHeight, 1);
        }

        foreach (Player player in Game.players)
        {
            player.coords = player.coords;
            player.transform.localScale = new Vector3(blockWidth, blockHeight, 1);
        }
    }


    public void UndoLastMove()
    {
        if (boardStates.Count <= 1)
            return;

        boardStates.RemoveAt(boardStates.Count - 1);
        boardStates[boardStates.Count - 1].Restore(this);

        Game.turn--;
    }


    public void SaveBoardState()
    {
        boardStates.Add(new BoardState(this));
    }


    #if UNITY_EDITOR
    void OnValidate()
    {
        Awake();

        UnityEditor.EditorApplication.delayCall += UpdateBoard;
    }
    #endif
}
