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
        foreach (Transform button in transform)
        {
            if (!button.GetComponent<Button>())
                continue;

            int stars = PlayerPrefs.GetInt(button.name + " Stars", 0);
            for (int star = 0; star < 3; star++)
            {
                Image starImage = button.Find("Stars/Star" + (star + 1) + "/Fill").GetComponent<Image>();

                starImage.color = star < stars ? Color.yellow : noStarColor;
            }
        }
    }
}
