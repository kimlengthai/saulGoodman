using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    [SerializeField] new ParticleSystem particleSystem;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isAnimating = false;

    Animator animator;

    [SerializeField] Vector2Int startingCoords;
    Queue<IEnumerator> coroutinesToPlay = new Queue<IEnumerator>();

    Vector2Int _coords = new Vector2Int(-1, -1);
    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            if (value == coords || isDead)
                return;
            
            Block block = Game.board.GetBlock(value);

            if (!Game.board.CanPlayerMoveTo(this, value))
            {
                AddAnimationToQueue(BumpIntoWallAnimation(value));
                if (block != null)
                    block.PlayerBump(this, value - coords);
            }
            else
            {
                Vector2Int direction = value - coords;
                _coords = value;

                AddAnimationToQueue(MovementAnimation(Game.board.GetBlockPosition(coords)));
                if (block != null)
                    block.PlayerEnter(this, direction);
            }
        }
    }

    public Vector3 truePosition
    {
        get { return Game.board.GetBlockPosition(coords); }
    }

    public float speed;

    public void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayerAnimationsLoop());
    }


    public void AddAnimationToQueue(IEnumerator animation)
    {
        coroutinesToPlay.Enqueue(animation);
    }


    public void ForceCoords(Vector2Int coords)
    {
        _coords = coords;
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


    public void OnTurnChange(Vector2Int direction)
    {
        coords += direction;
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


    #if UNITY_EDITOR
    void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += UpdatePlayerToStartingCoords;
    }
    #endif


    public void Die()
    {
        isDead = true;
        AddAnimationToQueue(DieAnimation());
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

        RaycastHit2D[] hits = Physics2D.LinecastAll(truePosition, player.truePosition);

        obstacles = new Block[hits.Length];
        for (int i = 0; i < hits.Length; i++)
            obstacles[i] = hits[i].collider.GetComponent<Block>();
        
        return hits.Length == 0;
    }


    public IEnumerator MovementAnimation(Vector2 target)
    {
        Vector2 startPosition = transform.position;
        float timeRatio = 0f;

        while (timeRatio < 1f)
        {
            timeRatio += Time.deltaTime * speed;
            transform.position = Vector2.Lerp(startPosition, target, timeRatio);
            yield return null;
        }

        transform.position = target;
    }

    IEnumerator BumpIntoWallAnimation(Vector2Int bumpingCoords)
    {
        transform.up = Game.board.GetBlockPosition(bumpingCoords) - (Vector2)transform.position;
        animator.SetTrigger("bumpIntoWall");

        while (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("Idle"))
        {
            yield return null;
        }
    }
}
