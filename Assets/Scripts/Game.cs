using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[ExecuteInEditMode]
public class Game : MonoBehaviour
{
    public static string[] levels = new string[]
    {
        "Tutorial 1", "Tutorial 2", "Tutorial 3", "Tutorial 4",
        /*"Level 1.1", "Level 1.2", "Level 1.3",*/ "Level 1.4",
        // "Level 2.1", "Level 2.2", "Level 2.3", "Level 2.4",
        // "Level 3.1", "Level 3.2", "Level 3.3", "Level 3.4",
        "Level 4.1", //"Level 4.2", "Level 4.3", "Level 4.4",
    };
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
            ui.UpdateTurnText();

            if (IsGameFinished(out bool won))
            {
                isPaused = true;

                if (won)
                {
                    print(mainPlayer.coords + " " + chasedPlayer.coords);
                    PlayerPrefs.SetInt("Score", CalcScore());
                    SceneManager.LoadScene("LevelCleared", LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
                }
            }
        }
    }

    [SerializeField] int _threeStarsTurns;
    public static int threeStarsTurns;

    [SerializeField] int _twoStarsTurns;
    public static int twoStarsTurns;

    [SerializeField] int _oneStarTurns;
    public static int oneStarTurns;

    public static Player[] players;

    public void Awake()
    {
        board = _board;
        ui = _ui;
        mainPlayer = _mainPlayer;
        chasedPlayer = _chasedPlayer;
        threeStarsTurns = _threeStarsTurns;
        twoStarsTurns = _twoStarsTurns;
        oneStarTurns = _oneStarTurns;

        players = new Player[] { mainPlayer, chasedPlayer };
    }


    public void Start()
    {
        foreach (Player player in players)
            player.InitCoords();
        
        turn = 0;
        isPaused = false;
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
                mainPlayer.visibilityLine.startColor = Color.red;
                mainPlayer.visibilityLine.endColor = Color.red;
                player.Die();
            }

        }
    }


    static public int CalcScore()
    {
        if (turn <= threeStarsTurns)
            return 3;
        else if (turn <= twoStarsTurns)
            return 2;
        else if (turn <= oneStarTurns)
            return 1;
        return 0;
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

    #if UNITY_EDITOR
    void OnValidate()
    {
        Awake();
    }
    #endif
}
