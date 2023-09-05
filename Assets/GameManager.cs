using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool lose, win;
    public float timeScale = 1;


    public GameObject losePanel, winPanel;

    public AudioSource rain, ambient;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rain.Play();
        ambient.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (lose)
        {
            Debug.Log("Lose");
            Destroy(GameObject.FindGameObjectWithTag("Thunder"));
            Invoke("LoseGame", 2f);
        }

        if (win)
        {
            Debug.Log("Win");
            Destroy(GameObject.FindGameObjectWithTag("Thunder"));
            Invoke("WinGame", 1f);
        }
    }

    private void LoseGame()
    {
        Time.timeScale = timeScale;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        losePanel.SetActive(true);

        rain.Stop();
        ambient.Stop();
    }
    private void WinGame()
    {
        Time.timeScale = timeScale;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winPanel.SetActive(true);

        rain.Stop();
        ambient.Stop();
    }
}
