using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelMenu : MonoBehaviour
{
    [SerializeField] Color noStarColor;

    void Start()
    {
        Game.InitScores();

        foreach (Transform button in transform)
        {
            bool isLevelCreated = Game.scores.ContainsKey(button.name);
            for (int star = 0; star < 3; star++)
            {
                Image starImage = button.Find("Stars/Star" + (star + 1) + "/Fill").GetComponent<Image>();

                if (isLevelCreated && star < Game.scores[button.name].Item2)
                    starImage.color = Color.yellow;
                else
                    starImage.color = noStarColor;
            }
        }
    }
}
