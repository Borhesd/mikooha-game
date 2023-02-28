using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeDirection { Out = -1, In = 1 }

public class Fade : MonoBehaviour
{
    public Image screenOverlay;

    public float duration = 1f;

    public FadeDirection direction = FadeDirection.Out;

    void Start()
    {
        if (direction == FadeDirection.Out)
            Invoke(nameof(Out), 0.5f);

        if (direction == FadeDirection.In)
            Invoke(nameof(In), 0.5f);
    }

    private void Update()
    {
        if (screenOverlay.color.a == 0)
            screenOverlay.gameObject.SetActive(false);
    }

    public void In()
    {
        screenOverlay.gameObject.SetActive(true);

        screenOverlay.CrossFadeAlpha(0f, 0f, true);

        screenOverlay.CrossFadeAlpha(1f, duration, true);
    }

    public void Out()
    {
        screenOverlay.CrossFadeAlpha(1f, 0f, true);

        screenOverlay.CrossFadeAlpha(0f, duration, true);
    }
}
