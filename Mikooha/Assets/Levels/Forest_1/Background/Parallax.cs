using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    Transform followingTarget;

    [SerializeField]
    [Range(0f, 1f)]
    float parallaxStrength = 0.1f;

    [SerializeField]
    bool disableVerticalParallax;

    Vector3 targePreviousPosition;


    // Start is called before the first frame update
    void Start()
    {
        if (!followingTarget)
            followingTarget = Camera.main.transform;

        targePreviousPosition = followingTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        var delta = followingTarget.position - targePreviousPosition;

        if (disableVerticalParallax)
            delta.y = 0;

        targePreviousPosition = followingTarget.position;

        transform.position += delta * parallaxStrength;
    }
}
