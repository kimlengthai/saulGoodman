using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;

    Vector2 offset;

    [SerializeField] float blockWidth;
    [SerializeField] float blockHeight;

    Block[,] blocks;

    public void Awake()
    {
        blocks = new Block[width, height];
        Block.width = blockWidth;
        Block.height = blockHeight;
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
        block.transform.position = GetBlockPosition(coords);
    }


    public Vector2 GetBlockPosition(Vector2Int coords)
    {
        return new Vector2(coords.x * Block.width, coords.y * Block.height) + offset;
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
                block.OnTurnChange();
        }
    }
}
