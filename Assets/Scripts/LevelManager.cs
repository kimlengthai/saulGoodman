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


    public void UndoLastMoveOnGameOver()
    {
        SceneManager.UnloadSceneAsync("GameOver");
        Game.board.RestoreLastBoardState();
        Game.isPaused = false;
        foreach (LineRenderer line in Game.visibilityLines.Keys)
            Game.InitLineRenderer(line);
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


    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene("Level Menu");
    }
}
