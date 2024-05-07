using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


[ExecuteInEditMode]
public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberOfTurnsText;
    [SerializeField] GameObject threeStars;
    [SerializeField] TextMeshProUGUI threeStarsText;
    [SerializeField] Image threeStarsCross;
    [SerializeField] GameObject twoStars;
    [SerializeField] TextMeshProUGUI twoStarsText;
    [SerializeField] Image twoStarsCross;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image turnProgressBar;


    public void Start()
    {
        UpdateTurnText();
        UpdateLevelText();
        UpdateTurnNeededText();
    }

    public void UpdateTurnText()
    {
        numberOfTurnsText.text = Game.turn.ToString();

        if (Game.turn > Game.threeStarsTurns)
        {
            threeStarsCross.enabled = true;
            if (Game.turn > Game.twoStarsTurns)
                twoStarsCross.enabled = true;
        }

        turnProgressBar.fillAmount = (float)Game.turn / Game.twoStarsTurns;
    }


    void UpdateTurnNeededText()
    {
        threeStars.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (float)Game.threeStarsTurns / Game.twoStarsTurns * threeStars.transform.parent.GetComponent<RectTransform>().rect.width, 1);
        threeStarsText.text = Game.threeStarsTurns.ToString();
        threeStarsCross.enabled = false;

        twoStars.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, twoStars.transform.parent.GetComponent<RectTransform>().rect.width, 1);
        twoStarsText.text = Game.twoStarsTurns.ToString();
        twoStarsCross.enabled = false;
    }


    public void UpdateLevelText()
    {
        levelText.text = Game.board.levelName.ToString();
    }


    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }
}
