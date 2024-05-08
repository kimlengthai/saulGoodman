using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCleared : MonoBehaviour
{
    [SerializeField] Animation scoreAnimation;
    [SerializeField] Image star1;
    [SerializeField] Image star2;
    [SerializeField] Image star3;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI bestScoreText;

    [SerializeField] GameObject scores;
    [SerializeField] GameObject infos;


    public void Start()
    {
        Image[] starsImages = new Image[] { star1, star2, star3 };
        int stars = Game.CalcStars();

        scoreText.text = "Moves : " + Game.turn.ToString();
        bestScoreText.text = "Best : " + Game.GetBestScore(Game.board.levelName).ToString();

        for (int star = 0; star < starsImages.Length; star++)
            starsImages[star].enabled = star < stars;
        
        if (Game.board.levelName == Game.levels[0])
        {
            scores.SetActive(false);
            infos.SetActive(true);
        }
        else
        {
            scores.SetActive(true);
            infos.SetActive(false);
        }
    }
}
