using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    bool selling = false;
    float distance;
    public GameObject player;
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
            selling = true;
        }
    }



    
}
