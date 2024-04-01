using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Board : MonoBehaviour
{

    public int width;
    public int height;

    Vector2 offset;

    [SerializeField] float blockWidth;
    [SerializeField] float blockHeight;

    [SerializeField] GameObject emptyBlockPrefab;
    [SerializeField] GameObject backgroud;
    [SerializeField] GameObject blocksParent;

    Block[,] blocks;

    public void Awake()
    {
        blocks = new Block[width, height];
        ResetOffset();
    }


    void ResetOffset()
    {
        offset = new Vector2(
            transform.position.x - width * blockWidth / 2 + blockWidth / 2,
            transform.position.y - height * blockHeight / 2 + blockHeight / 2
        );
    }


    bool IsInsideBoard(Vector2Int coords)
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


    public void RemoveBlock(Vector2Int coords)
    {
        if (!IsInsideBoard(coords))
        {
            Debug.LogError("Trying to remove a block outside the board");
            return;
        }

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


    public Vector2 GetBlockPosition(Vector2Int coords)
    {
        return new Vector2(coords.x * blockWidth, coords.y * blockHeight) + offset;
    }


    public bool CanPlayerMoveTo(Vector2Int coords)
    {
        if (!IsInsideBoard(coords))
            return false;
        
        Block block = GetBlock(coords);

        if (block == null)
            return true;
        
        return block.CanPlayerMoveInside();
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

        DestroyImmediate(backgroud);

        backgroud = new GameObject("Background");
        backgroud.transform.parent = transform;
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newBlock = Instantiate(emptyBlockPrefab, backgroud.transform);
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


    void OnValidate()
    {
        Awake();

        UnityEditor.EditorApplication.delayCall += UpdateBoard;
    }
}
