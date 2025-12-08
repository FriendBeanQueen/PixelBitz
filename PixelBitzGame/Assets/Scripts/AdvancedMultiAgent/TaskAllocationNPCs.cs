using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;


public class NPCAssignment
{
    public string roleclass;
    private GameObject npc;
    public NPCAssignment(string role, GameObject npcs)
    {
        roleclass = role;
        npc = npcs;
    }


}


public class TaskAllocationNPCs : MonoBehaviour
{
    public GameObject player;
    public GameObject joiningpopup;
    string role;
    float distance;
    bool joining = false;
    bool joined = false;
    public static int numOfNPCs = 0;
    public List<NPCAssignment> taskedNPCs = new List<NPCAssignment>();
    public GameObject[] npcchars;


    // Start is called before the first frame update
    void Start()
    {

        for(int i=0; i<npcchars.Length; i++)
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

            taskedNPCs.Add(new NPCAssignment(role, npcchars[i]));
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.F) && distance < 3 && !joined)
        {
            joining = true;
        }

        if(joined)
        {
            Follow(distance);
        }
        else
        {

        }
    }

    public void PartyJoin()
    {
        joined = true;
    }

    public void NoParty()
    {
        joining = false;
    }

    public void Follow(float distance)
    {
        if (distance > 5)
        {
            
        }
        else
        {

        }
    }

    public void Attack(string role)
    {
        switch(role)
        {
            case "Melee":

                break;
            case "Mage":

                break;
            case "Ranger":

                break;
            case "Summoner":

                break;
        }
    }
}
