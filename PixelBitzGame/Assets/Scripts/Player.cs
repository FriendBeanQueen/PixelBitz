using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20f;
    Rigidbody rb;
    Vector3 move, velocity;
    Quaternion rotate = Quaternion.identity;
    float rotateY = 0;
    public static int Phealth = 100;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    void Update()
    {
        PlayerHealth(this.gameObject);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //attack
        //}

    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal") * 1;
        float vertical = Input.GetAxis("Vertical") * 1;
        move.Set(vertical, 0f, 0);
        move.Normalize();
        if (Inventory.usedSP)
        {
            velocity = rb.transform.forward * vertical * speed * 2 * Time.deltaTime;
        }
        else {
            velocity = rb.transform.forward * vertical * speed * Time.deltaTime;
        }
        rb.position += velocity;
        rotateY += horizontal * 3;
        rb.transform.rotation = Quaternion.Euler(0, rotateY, 0);
    }
    private void OnGUI()
    {
        GUI.Button(new Rect(900, 50, 85, 45), "Health: " + Phealth);
    }
    void PlayerHealth(GameObject p)
    {
        if (Phealth <= 0)
        {
            //p.gameObject.SetActive(false);
        }
    }
}
