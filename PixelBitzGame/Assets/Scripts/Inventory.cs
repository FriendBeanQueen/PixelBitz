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

    int inv1amount;
    int inv2amount;
    int inv3amount;
    int inv4amount;
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
        inv4.text = ": " + (string)inv4amount.ToString();
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
        }
    }

    public void UseAP()
    {
        if (inv2amount > 0)
        {
            inv2amount--;
        }
    }

    public void UseDP()
    {
        if (inv3amount > 0)
        {
            inv3amount--;
        }
    }

    public void UseInv4()
    {
        if (inv4amount > 0)
        {
            inv4amount--;
        }
    }

}
