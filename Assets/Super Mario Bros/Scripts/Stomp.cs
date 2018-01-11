using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    public GameObject goomba;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If the collision is from a player...
        if (col.tag == "Player")
        {
            goomba.GetComponent<Goomba>().stomp = true;
        }
        // If the collision is from a shell...
        if (col.tag == "Shell")
        {
            goomba.GetComponent<Goomba>().stomp = true;
        }
        // If the collision is from the kill plane...
        if (col.tag == "Kill Plane")
        {
            Destroy(goomba);
        }
    }
}