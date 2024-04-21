using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLaser : Block
{
    [SerializeField] Vector2Int direction;
    [SerializeField] GameObject beamPrefab;


    List<Vector2Int> GetLaserPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();

        if (direction == Vector2Int.zero)
            return path;

        Block block;
        Vector2Int currentCoords = coords;
        do
        {
            path.Add(currentCoords);
            currentCoords += direction;
            block = Game.board.GetBlock(currentCoords);
        }
        while (Game.board.IsInsideBoard(currentCoords) && (block == null || block.CanSeeThrough()));

        return path;
    }


    protected override void UpdateSprite()
    {
        base.UpdateSprite();

        transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));

        foreach (Transform child in transform)
            if (child.name == "BeamSprites")
                DestroyImmediate(child.gameObject);
        
        GameObject beamSprites = new GameObject("BeamSprites");
        beamSprites.transform.SetParent(transform);
        beamSprites.transform.localPosition = Vector3.zero;

        foreach (Vector2Int beamCoords in GetLaserPath())
        {
            GameObject beam = Instantiate(beamPrefab, beamSprites.transform);
            beam.transform.position = Game.board.GetBlockPosition(beamCoords);
            beam.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        }

        print("Updating sprite at " + coords + " with direction " + direction + " -> " + GetLaserPath().Count + " blocks long.");
    }


    protected override void OnBoardChange()
    {
        base.OnBoardChange();

        foreach (Vector2Int beamCoords in GetLaserPath())
            foreach (Player player in Game.players)
                if (player.coords == beamCoords)
                    player.Die();
    }


    public override Dictionary<string, object> GetData()
    {
        Dictionary<string, object> data = base.GetData();
        data["direction"] = direction;
        return data;
    }


    public override void SetData(Dictionary<string, object> data)
    {
        base.SetData(data);
        direction = (Vector2Int)data["direction"];
    }
}
