using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnText;


    public void Start()
    {
        UpdateTurnText();
    }

    public void UpdateTurnText()
    {
        turnText.text = "Turn : " + Game.turn;
    }
}
