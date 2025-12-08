using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Merchant : MonoBehaviour
{
    float distance;
    public GameObject player;
    public GameObject sellingmenu;

    public TextMeshProUGUI item1;
    public TextMeshProUGUI item2;
    public TextMeshProUGUI item3;
    public TextMeshProUGUI item4;

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
            sellingmenu.SetActive(true);
        }

        item1.text = (string)item1amount.ToString();
        item2.text = (string)item2amount.ToString();
        item3.text = (string)item3amount.ToString();
        item4.text = (string)item4amount.ToString();
    }

    public void Close()
    {
        sellingmenu.SetActive(false);
    }

    public void Buy(int itemnum)
    {
        switch (itemnum)
        {
            case 1:
                if(item1amount > 0)
                {
                    item1amount--;
                    Inventory.inv1amount++;
                }
                break;
            case 2:
                if (item2amount > 0)
                {
                    item2amount--;
                    Inventory.inv2amount++;
                }
                break;
            case 3:
                if (item3amount > 0)
                {
                    item3amount--;
                    Inventory.inv3amount++;
                }
                break;
            case 4:
                if (item4amount > 0)
                {
                    item4amount--;
                    Inventory.inv4amount++;
                }
                break;

        }
    }
    
}
