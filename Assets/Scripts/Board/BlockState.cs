using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BlockState
{
    string blockName = null;
    Dictionary<string, object> blockData = new Dictionary<string, object>();

    public BlockState(Block block)
    {
        if (block == null)
            return;

        blockName = block.blockName;
        blockData = block.GetData();
    }


    public void Restore()
    {
        if (blockName == null)
            return;
        
        Block block = GameObject.Instantiate(Game.blockNameToPrefab[blockName]).GetComponent<Block>();
        block.transform.parent = Game.board.blocksParent.transform;

        block.SetData(blockData);
        block.shouldInit = false;
    }


    public override string ToString()
    {
        return blockName;
    }
}
