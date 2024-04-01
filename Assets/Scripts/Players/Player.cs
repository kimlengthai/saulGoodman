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

    Vector2Int _coords = new Vector2Int(-1, -1);
    public Vector2Int coords
    {
        get { return _coords; }
        set
        {
            if (value == coords || isDead)
                return;

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

    public float speed;

    public void Start()
    {
        coords = startingCoords;
        animator = GetComponent<Animator>();
    }


    public void OnTurnChange(Vector2Int direction)
    {
        coords += direction;
    }


    void UpdatePlayer()
    {
        if (this == null) return;

        coords = startingCoords;
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
