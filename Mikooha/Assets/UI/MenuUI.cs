using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public PauseMenu pauseMenu;

    public void ExitGame() => Application.Quit();

    public void ResumeGame()
    {
        pauseMenu.Pause = false;
    }
}
