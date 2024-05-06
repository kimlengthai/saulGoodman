using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    [SerializeField] new ParticleSystem particleSystem;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isAnimating = false;

    Animator animator;
    public AudioSource audioSource;

    [SerializeField] Vector2Int startingCoords;
    Queue<IEnumerator> coroutinesToPlay = new Queue<IEnumerator>();
    Queue<Action> actionsToPlay = new Queue<Action>();

    [HideInInspector] public bool isSliding = false;

    Vector2Int _coords = new Vector2Int(-1, -1);
    public Vector2Int coords
    {
        get => _coords;
        private set => _coords = value;
    }

    public Vector3 truePosition
    {
        get { return Game.board.GetBlockPosition(coords); }
    }

    public float speed;

    public void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        StartCoroutine(PlayerAnimationsLoop());
    }


    public void Init()
    {
        isDead = false;
        isAnimating = false;
        isSliding = false;
        coords = startingCoords;
    }


    Action MovementAction(Vector2Int direction, bool animate)
    {
        return () => 
        {
            Vector2Int nextCoords = coords + direction;
            Block block = Game.board.GetBlock(nextCoords);

            if (!Game.board.CanPlayerMoveTo(this, nextCoords, direction))
            {
                if (animate)
                    QueueAnimation(BumpIntoWallAnimation(direction));
                
                if (block != null)
                    block.PlayerBump(this, direction, animate);
            }
            else
            {
                coords = nextCoords;
                
                if (animate)
                    QueueAnimation(MovementAnimation(Game.board.GetBlockPosition(coords)));

                if (block != null)
                    block.PlayerEnter(this, direction, animate);
            }
        };
    }


    public void QueueAction(Action action)
    {
        actionsToPlay.Enqueue(action);
    }


    public void QueueMove(Vector2Int direction, bool animate)
    {
        QueueAction(MovementAction(direction, animate));
    }


    public void Move(Vector2Int direction, bool animate)
    {
        MovementAction(direction, animate)();
    }


    public void QueueAnimation(IEnumerator animation)
    {
        coroutinesToPlay.Enqueue(animation);
    }


    public void ForceCoords(Vector2Int newCoords)
    {
        coords = newCoords;
    }


    IEnumerator PlayerAnimationsLoop()
    {
        while (true)
        {
            while (coroutinesToPlay.Count == 0)
            {
                isAnimating = false;
                yield return null;
            }
            
            isAnimating = true;
            yield return StartCoroutine(coroutinesToPlay.Dequeue());
        }
    }


    public void UpdatePlayerCoords()
    {
        transform.position = truePosition;
    }


    public void UpdatePlayerToStartingCoords()
    {
        if (this == null) return;

        ForceCoords(startingCoords);
        UpdatePlayerCoords();
    }


    public bool HasActions()
    {
        return actionsToPlay.Count > 0;
    }


    public void DoNextAction()
    {
        if (actionsToPlay.Count == 0) return;

        actionsToPlay.Dequeue()();
    }


    #if UNITY_EDITOR
    void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += UpdatePlayerToStartingCoords;
    }
    #endif


    public void Die(bool animate)
    {
        isDead = true;
        if (animate)
            QueueAnimation(DieAnimation());
    }


    IEnumerator DieAnimation()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(particleSystem.main.duration);
    }


    public bool CanSeePlayer(Player player, out Block[] obstacles)
    {
        obstacles = null;

        if (player == this)
            return true;

        RaycastHit2D[] hits = Physics2D.LinecastAll(truePosition, player.truePosition, LayerMask.GetMask("Blocks"));

        obstacles = new Block[hits.Length];
        for (int i = 0; i < hits.Length; i++)
            obstacles[i] = hits[i].collider.GetComponent<Block>();
        
        return hits.Length == 0;
    }


    public IEnumerator MovementAnimation(Vector2 target)
    {
        Vector2 startPosition = transform.position;
        float timeRatio = 0f;

        audioSource.Play();

        while (timeRatio < 1f)
        {
            timeRatio += Time.deltaTime * speed;
            transform.position = Vector2.Lerp(startPosition, target, timeRatio);
            yield return null;
        }

        transform.position = target;
    }

    IEnumerator BumpIntoWallAnimation(Vector2Int direction)
    {
        transform.up = (Vector2)direction;
        animator.SetTrigger("bumpIntoWall");

        float timeRatio = 0f;
        while (timeRatio < 1f)
        {
            timeRatio += Time.deltaTime * speed;
            yield return null;
        }
    }
}
