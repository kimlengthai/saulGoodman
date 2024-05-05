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
        Image[] starsImages = new Image[] { star1, star2, star3 };
        int stars = Game.CalcStars();

        for (int star = 0; star < starsImages.Length; star++)
            starsImages[star].enabled = star < stars;
    }
}
