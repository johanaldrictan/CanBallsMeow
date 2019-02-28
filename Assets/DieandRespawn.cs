using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DieandRespawn : MonoBehaviour
{
    private Vector3 respawnPosition = new Vector3(-0.1098735f, 10, 0);
    private bool blockDeathEvent = false; //To make sure DieAndRespawn() doesn't continually get called. Will later move from Update() to only fire on event
    public int stocks = 3; // 9 if we wanna be techinically correct
    SpriteRenderer sr; //To hide the game object when they are KOd and to render them again when they respawn.
    public CinemachineVirtualCamera vcam;
    public CinemachineTargetGroup vgroup;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("CM vcam1") != null)
        {
            vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        }

        if(GameObject.Find("TargetGroup1") != null)
        {
            vgroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Rough bound settings for player death
        if (transform.position.x >= 22 || transform.position.x <= -22 || transform.position.y <= -9.3)
        {
            //Found similar code online, seems to work
            StartCoroutine(DieAndRespawn());
        }
    }

    /*Check if the death event has been fired, then decrement stock and respawn after a one second delay. Die if necessary*/
    IEnumerator DieAndRespawn()
    {
        if (!blockDeathEvent)
        {
            blockDeathEvent = true;
            stocks -= 1;
            if (stocks <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                sr = GetComponent<SpriteRenderer>();
                sr.enabled = false;
                vcam.LookAt = null;
                yield return new WaitForSeconds(0.2f);
                transform.position = respawnPosition;
                transform.rotation = Quaternion.identity;
                sr.enabled = true;
                vcam.LookAt = vgroup.transform;
                blockDeathEvent = false;
            }
        }
        
    }
}
