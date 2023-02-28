using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public MenuUI menuUI;
    public PlayerController player;
    public Fade fade;
    
    public bool Pause 
    {
        set => SetPauseState(value);
        get => pause; 
    }

    private bool pause;

    // Start is called before the first frame update
    void Start()
    {
        menuUI.gameObject.SetActive(false);
        pause = false;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause = !Pause;
    }

    private void SetPauseState(bool value)
    {
        if (value)
        {
            menuUI.gameObject.SetActive(true);
            player.DisableMovement();
            pause = true;
            Cursor.visible = true;
            fade.In();
        }
        else
        {
            menuUI.gameObject.SetActive(false);
            player.EnableMovement();
            pause = false;
            Cursor.visible = false;
            fade.Out();
        }
    }
}
