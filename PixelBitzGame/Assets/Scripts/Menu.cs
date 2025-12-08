using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject inventory;
    public GameObject controls;
    public GameObject minimenu;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu.gameObject.SetActive(false);
        inventory.SetActive(false);
        controls.SetActive(false);
        minimenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            menu.gameObject.SetActive(true);
            inventory.SetActive(false);
            controls.SetActive(false);
            minimenu.SetActive(false);
        }
    }

    public void Inventory()
    {
        menu.gameObject.SetActive(false);
        inventory.SetActive(true);
    }

    public void Controls()
    {
        menu.gameObject.SetActive(false);
        controls.SetActive(true);
    }

    public void Close()
    {
        menu.gameObject.SetActive(false);
        minimenu.SetActive(true);
    }

    public void CloseControls()
    {
        controls.SetActive(false);
        menu.gameObject.SetActive(true);
    }
}
