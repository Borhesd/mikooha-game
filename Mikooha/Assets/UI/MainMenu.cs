using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public string startScene;
    public float fadeUIDuration = 1f;

    private bool isFade = false;
    private bool faded = false;
    private CanvasGroup canvasGroup;

    private Action _action;

    private void Start()
    {
        Cursor.visible = true;
        canvasGroup = GetComponentInChildren<CanvasGroup>();   
    }

    private void Update()
    {
        if (isFade)
            Fade();

        if (faded)
        {
            faded = false;
            _action();
        }
    }

    public void ExitGame()
    {
        isFade = true;
        _action = Quit;
    }

    private void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        isFade = true;
        //SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);
        //_action = NextScene;
        _action = () => { SceneManager.LoadScene(startScene); };
    }

    private void NextScene()
    {
        var next = SceneManager.GetSceneByName(startScene);
        var current = SceneManager.GetActiveScene();

        SceneManager.UnloadSceneAsync(current);
        SceneManager.SetActiveScene(next);
    }

    private void Fade()
    {
        canvasGroup.alpha -= fadeUIDuration * Time.deltaTime;

        if (canvasGroup.alpha == 0)
        {
            isFade = false;
            faded = true;
        }
    }
}
