using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;

public class TaskAllocationNPCs : MonoBehaviour
{
    public GameObject player;
    string role;
    float distance;
    bool joining = false;
    public GameObject[] taskedNPCs = new GameObject[0];

    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.F) && distance < 3)
        {
            joining = true;
        }


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
        joining = false;
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
