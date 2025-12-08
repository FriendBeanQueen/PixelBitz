using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int bossHP = 100;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SummonMinion()
    {
        int minionnum = Random.Range(1, 3);
        switch(minionnum)
        {
            case 1:
                //summon minion 1
                break;
            case 2:
                //summon minion 2
                break;
            case 3:
                //summon minion 3
                break;
        }
    }
}
