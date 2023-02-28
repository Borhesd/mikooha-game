using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    public Fade fade;
    public List<AudioController> audioControllers;
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController.DisableMovement();

        fade.In();

        foreach (var controller in audioControllers)
            controller.FadeIn();

        Invoke(nameof(LoadMainMenu), 2);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
