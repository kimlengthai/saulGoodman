using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(Game.board.levelName);
    }

    public void UndoLastMove()
    {
        if (Game.isPaused) return;
        Game.board.UndoLastMove();
    }

    public void LoadNextLevel()
    {
        int levelIndex = System.Array.IndexOf(Game.levels, Game.board.levelName);
        if (levelIndex >= Game.levels.Length - 1)
        {
            LoadMenu();
            return;
        }

        string nextLevel = Game.levels[levelIndex + 1];
        SceneManager.LoadScene(nextLevel);
    }

    public void LoadLevel()
    { 
        Button button = GetComponent<Button>();
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        string level = buttonText.text;
        SceneManager.LoadScene(level);
    }

    public void MainMenuButtonLoader(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Level Menu");
    }
}
