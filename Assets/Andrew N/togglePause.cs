using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePause : MonoBehaviour
{
    bool paused = false;
    public KeyCode pause;
    GameObject child;
    GameObject originalGameObject = GameObject.Find("MainObj");
    GameObject[] pauseObjects;

    void Start() {
        //child = originalGameObject.transform.GetChild(0).gameObject;
        hidePaused();
    }

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
            //child.SetActive(false);
            hidePaused();
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            showPaused();
            //child.SetActive(true);
            return (true);
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

}

/*
 using System;
 using UnityEngine;
 
 public class pause : MonoBehaviour
 {
     bool paused = false;
 
     void Update()
     {
         if(Input.GetButtonDown("pauseButton"))
             paused = togglePause();
     }
     
     void OnGUI()
     {
         if(paused)
         {
             GUILayout.Label("Game is paused!");
             if(GUILayout.Button("Click me to unpause"))
                 paused = togglePause();
         }
     }
     
     bool togglePause()
     {
         if(Time.timeScale == 0f)
         {
             Time.timeScale = 1f;
             return(false);
         }
         else
         {
             Time.timeScale = 0f;
             return(true);    
         }
     }
 }
     */
