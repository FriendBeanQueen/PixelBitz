using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class NPCs : MonoBehaviour
{
    float distance;
    public GameObject player;
    public GameObject npctext;

    public TextMeshPro npcname;
    public TextMeshPro npcdescription;
    // Start is called before the first frame update
    void Start()
    {
        //npcname = RandomName();
        //npcdescription = RandomText();


    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.F) && distance < 3)
        {
            npctext.SetActive(true);
        }
    }

    public void Close()
    {
        npctext.SetActive(false);
    }

    //string RandomText()
    //{
    //    string randomtext = "";




    //    return randomtext;
    //}

    //string RandomName()
    //{
    //    string name = "";



    //    return name;
    //}
}
