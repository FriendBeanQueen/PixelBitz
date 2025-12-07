using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    bool selling = false;
    float distance;
    public GameObject player;
    public GameObject sellingmenu;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;

    int item1amount;
    int item2amount;
    int item3amount;
    int item4amount;
    // Start is called before the first frame update
    void Start()
    {
        item1amount = (int)Random.Range(1, 10);
        item2amount = (int)Random.Range(1, 10);
        item3amount = (int)Random.Range(1, 10);
        item4amount = (int)Random.Range(1, 10);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.F) && distance < 3)
        {
            selling = true;
        }
    }

    void Close()
    {
        selling = false;
    }

    void Buy(int amount)
    {
        if (amount == 0)
        {
            amount = 0;
        }
        else {
            amount--;
        }
    }
    
}
