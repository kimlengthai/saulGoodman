using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(Game.board.levelName);
    }


    public void UndoLastMove()
    {
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


    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
