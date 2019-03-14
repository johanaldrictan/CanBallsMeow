using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DieandRespawn : MonoBehaviour
{
    private PlayerController2 player1;
    private PlayerController2 player2;
    public CinemachineVirtualCamera vcam;
    public CinemachineTargetGroup vgroup;

    // Start is called before the first frame update
    void Start()
    {
        /*
        player1 = GameObject.Find("Orange Tabby Cat").GetComponent<PlayerController2>();
        player2 = GameObject.Find("Gray Tabby Cat 2").GetComponent<PlayerController2>();
       
 
        if (GameObject.Find("CM vcam1") != null)
        {
            vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        }

        if(GameObject.Find("TargetGroup1") != null)
        {
            vgroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*Check if the death event has been fired, then decrement stock and respawn after a one second delay. Die if necessary*/
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController2 player = collision.gameObject.GetComponent<PlayerController2>();
        player.fireDeath();
    }
    */
}
