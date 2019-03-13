using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePause : MonoBehaviour
{
    bool paused = false;
    public KeyCode pause;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pause))
            paused = togglePauseFunc();

    }

    bool togglePauseFunc()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            return (true);
        }
    }

}
