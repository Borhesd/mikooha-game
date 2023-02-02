using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            var respawn = GameObject.FindGameObjectWithTag("Respawn");

            if (respawn != null)
                respawn.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
        }
    }
}
