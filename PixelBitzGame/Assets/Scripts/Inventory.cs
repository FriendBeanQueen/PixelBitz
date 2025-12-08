using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject menu;
    public GameObject inventory;
    public GameObject minimenu;

    public TextMeshProUGUI inv1;
    public TextMeshProUGUI inv2;
    public TextMeshProUGUI inv3;
    public TextMeshProUGUI inv4;

    public static int inv1amount;
    public static int inv2amount;
    public static int inv3amount;
    public static int inv4amount;

    float timer;
    bool itemused = false;
    public static bool usedDP = false;
    public static bool usedAP = false;
    public static bool usedSP = false;

    // Start is called before the first frame update
    void Start()
    {
        inventory.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.gameObject.SetActive(true);
        }

        inv1.text = "Life Potion: " + (string)inv1amount.ToString();
        inv2.text = "Attack Potion: " + (string)inv2amount.ToString();
        inv3.text = "Defense Potion: " + (string)inv3amount.ToString();
        inv4.text = "Speed Potion: " + (string)inv4amount.ToString();

        while(itemused)
        {
            timer = timer + Time.deltaTime;
            if(timer > 5)
            {
                itemused = false;
                usedAP = false;
                usedDP = false;
                usedSP = false;
            }
        }
    }

    public void Close()
    {
        inventory.gameObject.SetActive(false);
        minimenu.gameObject.SetActive(true);
    }

    public void Menu()
    {
        inventory.gameObject.SetActive(false);
        menu.SetActive(true);
        minimenu.gameObject.SetActive(false);
    }

    public void UseLP()
    {
        if (inv1amount>0)
        {
            inv1amount--;
            Player.Phealth += 5;

        }
    }

    public void UseAP()
    {
        if (inv2amount > 0)
        {
            inv2amount--;
            itemused = true;
            usedAP = true;
            //increased attack for 5 seconds


        }
    }

    public void UseDP()
    {
        if (inv3amount > 0)
        {
            inv3amount--;
            itemused = true;
            usedDP = true;
            //incrased defense for 5 seconds


        }
    }

    public void UseSP()
    {
        if (inv4amount > 0)
        {
            inv4amount--;
            itemused = true;
            usedSP = true;
            //increased speed for 5 seconds


        }
    }


}
