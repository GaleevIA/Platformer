using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIMain : MonoBehaviour
{
    public GameObject hero;

    int winCount = 5;
    int curCount = 0;

    void Start()
    {

    }

    void Update()
    {
        curCount = hero.GetComponent<Character>().curCount;

        
    }

    private void OnGUI()
    {
        GUI.backgroundColor = Color.white;
        GUI.color = Color.white;
        GUI.Box(new Rect(5, 60, 175, 25), $"Убить {winCount} противников: {curCount}");

        if (curCount >= winCount)
            EndGame();
    }

    private void EndGame()
    {
        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 70, 200, 120), "Победа!");
        if(GUI.Button(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 35, 180, 20), "Начать уровень заново"))
            SceneManager.LoadScene(1);
        if (GUI.Button(new Rect(Screen.width / 2 - 90, Screen.height / 2 + 0, 180, 20), "Закончить игру"))
            SceneManager.LoadScene(0);


    }
}
