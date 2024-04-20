using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] bool undo;
    [SerializeField] bool restart;
    [SerializeField] bool next;
    [SerializeField] bool menu;

    void OnUndo()
    {
        if (undo)
            levelManager.UndoLastMove();
    }


    void OnRestart()
    {
        if (restart)
            levelManager.RestartLevel();
    }


    void OnNext()
    {
        if (next)
            levelManager.LoadNextLevel();
    }


    void OnMenu()
    {
        if (menu)
            levelManager.LoadMenu();
    }
}
