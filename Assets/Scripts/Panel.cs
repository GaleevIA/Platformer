using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public Transform[] lives;
    public GameObject player;

    Character playerScript;

    void Start()
    {
        playerScript = player.GetComponent<Character>();

        lives = new Transform[transform.childCount];

        for (int i = 0; i < lives.Length; i++)
            lives[i] = transform.GetChild(i);
    }

    public void Refresh()
    {
        for (int i = 0; i < lives.Length; i++)
            lives[i].gameObject.SetActive(false);

        for (int i = 0; i < playerScript.health; i++)
            lives[i].gameObject.SetActive(true);       
    }
}
