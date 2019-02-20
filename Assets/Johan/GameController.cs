using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public PlayerController player1Prefab;
    public PlayerController player2Prefab;

    public CinemachineTargetGroup targetGroup;

    private PlayerController player1;
    private PlayerController player2;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer(player1Prefab, new Vector3(-5, -2, 0));
        SpawnPlayer(player2Prefab, new Vector3(5, -2, 0));
    }
    void SpawnPlayer(PlayerController player, Vector3 spawnLoc)
    {
        if(player.gameObject.tag.Equals("Player 1"))
        {
            player1 = Instantiate(player, spawnLoc, Quaternion.identity);
            targetGroup.AddMember(player1.transform, 1, 0);
        }
        else
        {
            player2 = Instantiate(player, spawnLoc, Quaternion.identity);
            targetGroup.AddMember(player2.transform, 1, 0);
        }
    }

}
