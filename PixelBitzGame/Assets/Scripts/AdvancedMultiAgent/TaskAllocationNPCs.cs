using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAllocationNPCs : MonoBehaviour
{
    public GameObject player;
    string role;
    // Start is called before the first frame update
    void Start()
    {
        int rolenum = (int)Random.Range(1, 4);
        switch (rolenum)
        {
            case 1:
                role = "Melee";
                break;
            case 2:
                role = "Mage";
                break;
            case 3:
                role = "Ranger";
                break;
            case 4:
                role = "Summoner";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void JoinParty()
    {

    }

    void NoParty()
    {

    }

    void Melee()
    {
        
    }

    void Mage()
    {

    }

    void Ranger()
    {

    }

    void Summoner()
    {

    }
}
