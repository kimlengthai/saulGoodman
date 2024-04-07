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


    static void OnPlayersTurnChange(Vector2Int playerDirection)
    {
        foreach (Player player in players)
        {
            player.OnTurnChange(playerDirection);
        }
    }


    static void CheckPlayersVisibility()
    {
        foreach (Player player in players)
        {
            if (!player.CanSeeEveryPlayers())
            {
                mainPlayer.visibilityLine.SetColors(Color.red, Color.red);
                player.Die();
            }

        }
    }


    static public void OnTurnChange(Vector2Int playerDirection)
    {
        if (isPaused)
            return;

        board.OnTurnChange();
        OnPlayersTurnChange(playerDirection);
        CheckPlayersVisibility();

        turn++;
    }

    void OnValidate()
    {
        Awake();
    }
}
