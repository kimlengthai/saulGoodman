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
    [SerializeField] Vector2Int _coords;

    Animator animator;

    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            transform.up = Game.board.GetBlockPosition(value) - (Vector2)transform.position;

            if (!Game.board.CanPlayerMoveTo(value))
            {
                animator.SetTrigger("bumpIntoWall");
                return;
            }
            
            _coords = value;
            StartCoroutine(MovementAnimation(Game.board.GetBlockPosition(coords)));
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
