using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;

public class NPCs : MonoBehaviour
{
    bool talking = false;
    float distance;
    public GameObject player;
    public GameObject npctext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.F) && distance < 3)
        {
            talking = true;
        }
    }

    void Close()
    {
        talking = false;
    }

    void RandomText()
    {

    }

    void RandomName()
    {

    }
}
