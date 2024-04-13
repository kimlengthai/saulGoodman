using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreakable : Block
{
    [SerializeField] int startingDurability = 1;

    int _durability;
    int durability
    {
        get => _durability;
        set
        {
            _durability = value;
            if (_durability <= 0)
            {
                Game.board.RemoveBlock(coords);
                Destroy(gameObject);
            }
        }
    }


    public override void Start()
    {
        base.Start();
        durability = startingDurability;
    }


    protected override void OnPlayerBump(Player player, Vector2Int playerDirection)
    {
        base.OnPlayerBump(player, playerDirection);
        durability--;
    }
}
