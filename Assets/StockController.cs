using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockController : MonoBehaviour
{
    public Image[] emptyHearts;
    private PlayerController2 player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController2>();   
    }

    // Update is called once per frame
    public void OnStockChange()
    {
        switch (player.stocks)
        {
            case 2:
                emptyHearts[2].gameObject.SetActive(true);
                break;
            case 1:
                emptyHearts[1].gameObject.SetActive(true);
                break;
            case 0:
                emptyHearts[0].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
