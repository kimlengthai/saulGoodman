using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Game : MonoBehaviour
{
    public static bool isInitialized = false;
    public static bool isPaused = false;

    [SerializeField] Board _board;
    public static Board board;

    [SerializeField] UI _ui;
    public static UI ui;

    [SerializeField] MainPlayer _mainPlayer;
    public static MainPlayer mainPlayer;

    [SerializeField] ChasedPlayer _chasedPlayer;
    public static ChasedPlayer chasedPlayer;

    private static bool isMainPlayerVisible = true;

    static int _turn = 0;
    public static int turn
    {
        get { return _turn; }
        set
        {
            _turn = value;
            ui.UpdateTurnRemainingText(maxTurns - _turn);

            if (IsGameFinished(out bool won))
            {
                isPaused = true;

                if (won)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelCleared", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
        }
    }

    [SerializeField] int _maxTurns;
    public static int maxTurns;

    public static Player[] players;

    public void Awake()
    {
        board = _board;
        ui = _ui;
        mainPlayer = _mainPlayer;
        chasedPlayer = _chasedPlayer;
        maxTurns = _maxTurns;
        players = new Player[] { mainPlayer, chasedPlayer };

        isInitialized = true;
    }

    static bool IsGameFinished(out bool won)
    {
        won = false;

        foreach (Player player in players)
            if (player.isDead)
                return true;

        if (mainPlayer.coords == chasedPlayer.coords)
        {
            won = true;
            return true;
        }

        if (turn >= maxTurns)
        {
            return true;
        }

        return false;
    }

    static void ActivateBlocks(Vector2Int playerDirection)
    {
        Dictionary<Block, List<Player>> bumpingBlocks = new Dictionary<Block, List<Player>>();
        Dictionary<Block, List<Player>> enteringBlocks = new Dictionary<Block, List<Player>>();
        Dictionary<Block, List<Player>> interactingBlocks = new Dictionary<Block, List<Player>>();

        foreach (Player player in players)
        {
            Block block = board.GetBlock(player.coords + playerDirection);
            if (block == null)
                continue;

            Dictionary<Block, List<Player>> dict = block.CanPlayerMoveInside() ? enteringBlocks : bumpingBlocks;

            if (!dict.ContainsKey(block))
                dict[block] = new List<Player>();
            dict[block].Add(player);

            if (!interactingBlocks.ContainsKey(block))
                interactingBlocks[block] = new List<Player>();
            interactingBlocks[block].Add(player);
        }

        foreach (Block block in enteringBlocks.Keys)
        {
            block.PlayerEnter(enteringBlocks[block], playerDirection);
        }

        foreach (Block block in bumpingBlocks.Keys)
        {
            block.PlayerBump(bumpingBlocks[block], playerDirection);
        }

        foreach (Block block in interactingBlocks.Keys)
        {
            block.PlayerInteract(interactingBlocks[block], playerDirection);
        }
    }

    static public void OnTurnChange(Vector2Int playerDirection)
    {
        if (isPaused)
            return;

        board.OnTurnChange();

        if (isInitialized)
        {
            if (isMainPlayerVisible)
            {
                mainPlayer.ToggleVisibility(true);
                chasedPlayer.ToggleVisibility(false);
            }
            else
            {
                mainPlayer.ToggleVisibility(false);
                chasedPlayer.ToggleVisibility(true);
            }

            isMainPlayerVisible = !isMainPlayerVisible;
        }

        ActivateBlocks(playerDirection);

        mainPlayer.OnTurnChange(playerDirection);
        chasedPlayer.OnTurnChange(playerDirection);

        turn++;
    }

    void OnValidate()
    {
        Awake();
    }
}
