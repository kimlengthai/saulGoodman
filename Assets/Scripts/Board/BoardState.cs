using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardState
{
    int width;
    int height;

    BlockState[,] blocksState;
    List<PlayerState> playersState = new List<PlayerState>();


    public BoardState(Board board)
    {
        width = board.width;
        height = board.height;

        blocksState = new BlockState[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                blocksState[x, y] = new BlockState(board.GetBlock(new Vector2Int(x, y)));

        foreach (Player player in Game.players)
            playersState.Add(new PlayerState(player));
    }


    public void Restore(Board board)
    {
        board.Clear();

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                blocksState[x, y].Restore();

        for (int i = 0; i < playersState.Count; i++)
            playersState[i].Restore(Game.players[i]);
    }


    public override string ToString()
    {
        string result = "";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
                result += blocksState[x, y].ToString();
            result += "\n";
        }

        return result;
    }
}
