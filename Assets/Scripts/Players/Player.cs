using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    [SerializeField] new ParticleSystem particleSystem;

    [HideInInspector] public bool isDead = false;

    Animator animator;

    [SerializeField] Vector2Int startingCoords;
    PriorityQueue<IEnumerator, float> coroutinesToPlay = new PriorityQueue<IEnumerator, float>();

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
                if (block != null)
                    block.PlayerBump(this, value - coords);
                coroutinesToPlay.Enqueue(BumpIntoWallAnimation(value), 0f);
            }
            else
            {
                Vector2Int direction = value - coords;
                _coords = value;

                if (block != null)
                    block.PlayerEnter(this, direction);
                coroutinesToPlay.Enqueue(MovementAnimation(Game.board.GetBlockPosition(coords)), 0f);
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
    }


    public void ForceCoords(Vector2Int coords)
    {
        _coords = coords;
        transform.position = Game.board.GetBlockPosition(coords);
    }


    public void InitCoords()
    {
        ForceCoords(startingCoords);
    }


    public IEnumerator StartAllAnimations()
    {
        PriorityQueue<IEnumerator, float> coroutines = coroutinesToPlay.Clone();
        coroutinesToPlay.Clear();

        foreach (IEnumerator coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }
    }


    public void OnTurnChange(Vector2Int direction)
    {
        coords += direction;
        StartCoroutine(StartAllAnimations());
    }


    void UpdatePlayer()
    {
        if (this == null) return;

        ForceCoords(startingCoords);
    }


    #if UNITY_EDITOR
    void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += UpdatePlayer;
    }
    #endif


    public void Die()
    {
        if (!Application.isPlaying) return;
        
        isDead = true;
        coroutinesToPlay.Enqueue(DieAnimation(), -1f);
    }


    IEnumerator DieAnimation()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(particleSystem.main.duration);
    }


    public bool CanSeePlayer(Player player)
    {
        if (player == this)
            return true;

        return Physics2D.Linecast(truePosition, player.truePosition).collider == null;
    }


    IEnumerator MovementAnimation(Vector2 target)
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
