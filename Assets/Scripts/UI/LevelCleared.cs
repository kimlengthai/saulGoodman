using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCleared : MonoBehaviour
{
    [SerializeField] Animation scoreAnimation;
    [SerializeField] Image star1;
    [SerializeField] Image star2;
    [SerializeField] Image star3;


    public void Start()
    {
        if (Game.turn > Game.threeStarsTurns)
        {
            star3.enabled = false;
            if (Game.turn > Game.twoStarsTurns)
            {
                star2.enabled = false;
                if (Game.turn > Game.oneStarTurns)
                    star1.enabled = false;
            }
        }
    }
}
