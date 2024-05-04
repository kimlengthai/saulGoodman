using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;


[ExecuteInEditMode]
public class Game : MonoBehaviour
{
    public static string[] levels = new string[]
    {
        "Tutorial 1", "Tutorial 2", "Tutorial 3", "Tutorial 4",
        "Level 1.1", "Level 1.2", "Level 1.3", "Level 1.4",
        "Level 2.1", "Level 2.2", "Level 2.3", //"Level 2.4",
        "Level 3.1", "Level 3.2", "Level 3.3", "Level 3.4",
        "Level 4.1", "Level 4.2", "Level 4.3", "Level 4.4",
    };

    public List<GameObject> blockPrefabs = new List<GameObject>();
    public static Dictionary<string, GameObject> blockNameToPrefab = new Dictionary<string, GameObject>();
    public static bool isPaused = false;
    public static Dictionary<LineRenderer, Player[]> visibilityLines = new Dictionary<LineRenderer, Player[]>();

    [SerializeField] Board _board;
    public static Board board;

    [SerializeField] UI _ui;
    public static UI ui;

    public static Game game;

    static int _turn = 0;
    public static int turn
    {
        get { return _turn; }
        set
        {
            _turn = value;
            ui.UpdateTurnText();

            game.StartCoroutine(OnGameFinish());
        }
    }

    [SerializeField] int _threeStarsTurns;
    public static int threeStarsTurns;

    [SerializeField] int _twoStarsTurns;
    public static int twoStarsTurns;

    [SerializeField] int _oneStarTurns;
    public static int oneStarTurns;

    [SerializeField] Player[] _players;
    public static Player[] players;

    public static Queue<IEnumerator> coroutinesToPlayAtEnd = new Queue<IEnumerator>();

    public void Awake()
    {
        game = this;
        board = _board;
        ui = _ui;
        threeStarsTurns = _threeStarsTurns;
        twoStarsTurns = _twoStarsTurns;
        oneStarTurns = _oneStarTurns;
        players = _players;

        foreach (GameObject blockPrefab in blockPrefabs)
        {
            Block block = blockPrefab.GetComponent<Block>();
            if (block == null)
                throw new Exception($"Block prefab {blockPrefab.name} does not have a Block component attached to it.");
            blockNameToPrefab[block.blockName] = blockPrefab;
        }
    }


    public void Start()
    {
        foreach (Player player in players)
            player.UpdatePlayerToStartingCoords();
        
        DestroyImmediate(GameObject.Find("Visibility Lines"));
        GameObject visibilityLinesObject = new GameObject("Visibility Lines");

        visibilityLines.Clear();
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = i + 1; j < players.Length; j++)
            {
                GameObject visibilityLine = new GameObject("Visibility Line", typeof(LineRenderer));
                visibilityLine.transform.parent = visibilityLinesObject.transform;

                LineRenderer lineRenderer = visibilityLine.GetComponent<LineRenderer>();
                InitLineRenderer(lineRenderer);
                visibilityLines.Add(lineRenderer, new Player[] { players[i], players[j] });
            }
        }
        UpdateVisibilityLines();

        StartCoroutine(StartGame());
    }


    public IEnumerator StartGame()
    {
        yield return null;

        board.UpdateBoard();

        turn = 0;
        isPaused = false;

        board.ResetBoardState();
    }


    static IEnumerator OnGameFinish()
    {
        yield return new WaitForFixedUpdate();

        if (!IsGameFinished(out bool won))
            yield break;

        isPaused = true;
        game.StartCoroutine(GameAnimationsEndLoop());

        if (won)
        {
            PlayerPrefs.SetInt("Score", CalcScore());
            coroutinesToPlayAtEnd.Enqueue(LevelClearedAnimation());
        }
        else
        {
            coroutinesToPlayAtEnd.Enqueue(GameOverAniamtion());
        }
    }


    static IEnumerator LevelClearedAnimation()
    {
        yield return null;
        SceneManager.LoadScene("LevelCleared", LoadSceneMode.Additive);
    }


    static IEnumerator GameOverAniamtion()
    {
        yield return null;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }


    public static void InitLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.sortingOrder = 1000;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }


    public void Update()
    {
        UpdateVisibilityLines();
    }


    void OnUp()
    {
        OnTurnChange(Vector2Int.up);
    }

    void OnDown()
    {
        OnTurnChange(Vector2Int.down);
    }

    void OnLeft()
    {
        OnTurnChange(Vector2Int.left);
    }

    void OnRight()
    {
        OnTurnChange(Vector2Int.right);
    }


    static void UpdateVisibilityLines()
    {
        foreach (KeyValuePair<LineRenderer, Player[]> visibilityLine in visibilityLines)
        {
            visibilityLine.Key.SetPosition(0, visibilityLine.Value[0].transform.position);
            visibilityLine.Key.SetPosition(1, visibilityLine.Value[1].transform.position);
        }
    }


    static bool AllPlayersHaveSameCoords()
    {
        if (players.Length == 0)
            return true;

        Vector2Int coords = players[0].coords;
        foreach (Player player in players)
            if (player.coords != coords)
                return false;
        return true;
    }


    static bool IsGameFinished(out bool won)
    {
        won = false;

        CheckPlayersVisibility();
        
        foreach (Player player in players)
            if (player.isDead)
                return true;
        
        if (AllPlayersHaveSameCoords())
        {
            won = true;
            return true;
        }

        return false;
    }


    static void CheckPlayersVisibility()
    {
        foreach (KeyValuePair<LineRenderer, Player[]> visibilityLine in visibilityLines)
        {
            if (!visibilityLine.Value[0].CanSeePlayer(visibilityLine.Value[1], out Block[] obstacles))
            {
                visibilityLine.Value[0].isDead = true;
                visibilityLine.Value[1].isDead = true;

                coroutinesToPlayAtEnd.Enqueue(PlayerLostVisibilityAnimation(visibilityLine, obstacles));
            }
        }
    }


    static IEnumerator PlayerLostVisibilityAnimation(KeyValuePair<LineRenderer, Player[]> visibilityLine, Block[] obstacles)
    {
        for (int i = 0; i < 5; i++)
        {
            visibilityLine.Key.startColor = Color.green;
            visibilityLine.Key.endColor = Color.green;
            foreach (Block obstacle in obstacles)
                obstacle.StartCoroutine(obstacle.ChangeSpriteColor(obstacle.defaultColor, 1 / 0.1f));
            yield return new WaitForSeconds(0.1f);
            visibilityLine.Key.startColor = Color.red;
            visibilityLine.Key.endColor = Color.red;
            foreach (Block obstacle in obstacles)
                obstacle.StartCoroutine(obstacle.ChangeSpriteColor(Color.red, 1 / 0.1f));
            yield return new WaitForSeconds(0.1f);
        }

        visibilityLine.Value[0].Die(animate: true);
        visibilityLine.Value[1].Die(animate: true);
    }


    static IEnumerator GameAnimationsEndLoop()
    {
        yield return null;

        while (players.Any(player => player.isAnimating))
            yield return null;
        
        while (coroutinesToPlayAtEnd.Count > 0)
            yield return game.StartCoroutine(coroutinesToPlayAtEnd.Dequeue());
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

        CalcNextTurn(playerDirection, animate: true);

        turn++;

        board.SaveBoardState();
    }


    static public void CalcNextTurn(Vector2Int playerDirection, bool animate)
    {
        board.OnTurnChange(animate);
        
        foreach (Player player in players)
            player.QueueMove(playerDirection, animate);
        
        while (players.Any(player => player.HasActions()))
            CalcNextTick(animate);
    }


    static public void CalcNextTick(bool animate)
    {
        foreach (Player player in players)
            player.DoNextAction();
        
        board.OnPlayersActionFinish(animate);
    }


    #if UNITY_EDITOR
    void OnValidate()
    {
        Awake();
    }
    #endif
}
