using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCleared : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;


    public void Start()
    {
        int score = PlayerPrefs.GetInt("Score");
        
        scoreText.text = "Score : " + score + " Stars!";
    }
}
