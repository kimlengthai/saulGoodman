using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI levelText;


    public void Start()
    {
        UpdateTurnText();
        UpdateLevelText();
    }

    public void UpdateTurnText()
    {
        turnText.text = "Turn : " + Game.turn;
    }


    public void UpdateLevelText()
    {
        levelText.text = "Level : " + Game.board.levelName;
    }
}
