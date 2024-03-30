using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    up,
    down,
    left,
    right
}


[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    [SerializeField] new ParticleSystem particleSystem;
    [SerializeField] Vector2Int _coords;

    [HideInInspector] public bool isDead = false;
    public Direction lastMoveDirection;

    Animator animator;

    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            Vector2Int dirVector = value - _coords;
            transform.up = Game.board.GetBlockPosition(value) - (Vector2)transform.position;

            if (!Game.board.CanPlayerMoveTo(value))
            {
                animator.SetTrigger("bumpIntoWall");
                print("Bump into wall");
                return;
            }

            if (dirVector.x > 0) lastMoveDirection = Direction.right;
            else if (dirVector.x < 0) lastMoveDirection = Direction.left;
            else if (dirVector.y > 0) lastMoveDirection = Direction.up;
            else if (dirVector.y < 0) lastMoveDirection = Direction.down;
            
            _coords = value;
            StartCoroutine(MovementAnimation(Game.board.GetBlockPosition(coords)));
        }
    }

    public Vector2Int DirectionToVector2Int(Direction direction)
    {
        switch (direction)
        {
            case Direction.up:
                return new Vector2Int(0, 1);
            case Direction.down:
                return new Vector2Int(0, -1);
            case Direction.left:
                return new Vector2Int(-1, 0);
            case Direction.right:
                return new Vector2Int(1, 0);
            default:
                return Vector2Int.zero;
        }
    }

    [SerializeField] float speed;

    public void Start()
    {
        coords = _coords;
        animator = GetComponent<Animator>();
    }


    public void OnTurnChange(Vector2Int direction)
    {
        coords += direction;
    }


    void UpdatePlayer()
    {
        if (this == null) return;

        coords = _coords;
    }


    void OnValidate()
    {
        // Check if the block is in the scene or is a prefab
        if (transform.parent == null) return;

        UnityEditor.EditorApplication.delayCall += UpdatePlayer;
    }


    public void Die()
    {
        isDead = true;
        particleSystem.Play();
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
}
