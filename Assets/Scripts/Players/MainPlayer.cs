using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Player
{
    private SpriteRenderer spriteRenderer;
    public bool isVisible = true; // Track visibility status

    public LineRenderer visibilityLine;

    new void Start()
    {
        base.Start();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ToggleVisibility(true); // Ensure visibility is set to true initially

        visibilityLine = GetComponent<LineRenderer>();
        UpdateVisibilityLine();
    }


    public override void Update()
    {
        base.Update();

        UpdateVisibilityLine();
    }


    void UpdateVisibilityLine()
    {
        visibilityLine.SetPosition(0, transform.position);
        visibilityLine.SetPosition(1, Game.chasedPlayer.transform.position);
    }


    void OnUp()
    {
        Game.OnTurnChange(Vector2Int.up);
    }

    void OnDown()
    {
        Game.OnTurnChange(Vector2Int.down);
    }

    void OnLeft()
    {
        Game.OnTurnChange(Vector2Int.left);
    }

    void OnRight()
    {
        Game.OnTurnChange(Vector2Int.right);
    }

    // Toggle visibility method
    public void ToggleVisibility(bool visible)
    {
        spriteRenderer.enabled = visible;
        isVisible = visible;
    }
}
