using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasedPlayer : Player
{
    private SpriteRenderer spriteRenderer;
    public bool isVisible = true; // Track visibility status

    new void Start()
    {
        base.Start();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ToggleVisibility(true); // Ensure visibility is set to true initially
    }

    // Toggle visibility method
    public void ToggleVisibility(bool visible)
    {
        spriteRenderer.enabled = visible;
        isVisible = visible;
    }
}
