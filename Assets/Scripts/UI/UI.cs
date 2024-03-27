using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnRemainingText;


    public void Start()
    {
        UpdateTurnRemainingText(Game.maxTurns);
    
    }

    public void UpdateTurnRemainingText(int turnRemaining)
    {
        turnRemainingText.text = "Turn Remaining : " + turnRemaining;
    }
}
